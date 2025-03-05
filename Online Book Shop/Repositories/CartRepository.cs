﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Book_Shop.Models;


namespace Online_Book_Shop.Repositories
{
    public class CartRepository: ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public CartRepository(ApplicationDbContext db,IHttpContextAccessor httpContextAccessor,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int bookId,int quantity)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            { 
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                //cart details section
                var cartItem = _db.CartDetails
                    .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if(cartItem is not null)
                {
                    cartItem.Quantity+=quantity;
                }
                else
                {
                    var book = _db.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity=quantity,
                        UnitPrice=book.Price
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {}
            var totalItemCount = await GetCartItemCount(userId);
            return totalItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            //using var transaction = _db.Database.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Invalid Cart");
                }
                //cart details section
                var cartItem = _db.CartDetails
                            .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                    throw new Exception("No items in Cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
               
                _db.SaveChanges();
            }
            catch (Exception ex)
            { 
            }
            var totalItemCount = await GetCartItemCount(userId);
            return totalItemCount;
        }
        public async Task<ShoppingCart> GetUserCart(){
            var userId = GetUserId();
            if (userId == null)
            {
                throw new Exception("Invalid userid");
            }
            var shoppingCart = await _db.ShoppingCarts
                                .Include(a => a.CartDetails)
                                .ThenInclude(a => a.Book)
                                .ThenInclude(a => a.Genre)
                                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async  Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> GetCartItemCount(string userId="")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                        join cartDetail in _db.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        where cart.UserId==userId
                        select new { cartDetail.Id }).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                //move data from cartDetail to order and orderDetail
                //entry->order,orderdetail
                //remove data from ->cartdetail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var pendingRecord = _db.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new Exception("Order Status does not have Pending status");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name=model.Name,
                    Email=model.Email,
                    MobileNumber=model.MobileNumber,
                    Address=model.Address,
                    PaymentMethod=model.PaymentMethod,
                    IsPaid=false,
                    OrderStatusId = pendingRecord.Id//pending by default
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetails
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        QuantityId = item.Quantity,
                        UnitPrices = item.UnitPrice
                    };
                    _db.OrderDetailss.Add(orderDetail);
                }
                _db.SaveChanges();
                //Removing the Data from cartDetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
        
    }
}
