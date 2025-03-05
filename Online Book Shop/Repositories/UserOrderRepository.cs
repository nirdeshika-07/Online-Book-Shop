using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Online_Book_Shop.Repositories
{
    public class UserOrderRepository: IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _db.Orders.FindAsync(data.OrderId);
            if(order==null)
            {
                throw new InvalidOperationException($"order within id:{data.OrderId} could not be found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _db.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order =await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order within id: {orderId} could not be found");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                        .Include(x => x.OrderStatus)
                        .Include(x => x.OrderDetails)
                        .ThenInclude(x => x.Book)
                        .ThenInclude(x => x.Genre).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }
            return await orders.ToListAsync();
        }

        //public async Task<IEnumerable<Order>> UserOrders()
        //{
        //    var userId = GetUserId();
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        throw new Exception("User is not logged-in");
        //    }
        //    var orders = await _db.Orders
        //                .Include(x => x.OrderStatus)
        //                .Include(x => x.OrderDetails)
        //                .ThenInclude(x => x.Book)
        //                .ThenInclude(x => x.Genre)
        //                .Where(a => a.UserId == userId)
        //                .ToListAsync();
        //    return orders;
        //}
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
