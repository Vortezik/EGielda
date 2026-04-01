using EGielda.Models;
using Microsoft.EntityFrameworkCore;
namespace EGielda.Data
{
    public class EgieldaDbContext : DbContext
    {
        public EgieldaDbContext(DbContextOptions<EgieldaDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
