using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Book_Shop.Models;

namespace Online_Book_Shop.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Book> Books{get;set;}
    public DbSet<CartDetail> CartDetails{get;set;}
    public DbSet<Genre> Genres{get;set;}
    public DbSet<ShoppingCart> ShoppingCarts{get;set;}
    public DbSet<Order> Orders{get;set;}
    public DbSet<OrderDetails> OrderDetailss{get;set;}
    public DbSet<OrderStatus> OrderStatuses{get;set;}

}
