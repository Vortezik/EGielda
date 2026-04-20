using EGielda.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EGielda.Data
{
    public static class DbSeeder
    {
        public static void Seed(EgieldaDbContext context)
        {
            context.Database.Migrate();

            if (context.Categories.Any() || context.Products.Any() || context.Users.Any())
            { 
                return; 
            }


            var admin = new User
            {
                Name = "Admin",
                Email = "admin@egielda.com",
                PasswordHash = HashPassword("admin123"),
                Role = "Admin"
            };

            var user1 = new User
            {
                Name = "Adrian",
                Email = "adrian@egielda.com",
                PasswordHash = HashPassword("adi123"),
                Role = "User"
            };

            context.Users.AddRange(admin, user1);
            context.SaveChanges();

            var cat1 = new Category { Name = "RTV" };
            var cat2 = new Category { Name = "Mobile" };
            var cat3 = new Category { Name = "Automotive" };

            context.Categories.AddRange(cat1, cat2, cat3);
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "IPhone 15",
                    Description = "Phone by Apple",
                    Price = 2599.00m,
                    ImageUrl = "https://files.refurbed.com/ii/iphone-15-pro-1694590053.jpg?t=fitdesign&h=600&w=800",
                    CategoryId = cat2.Id
                },
                new Product
                {
                    Name = "SHARP",
                    Description = "4K Ultra HD Powered by TiVo Smart TV",
                    Price = 2259.00m,
                    ImageUrl = "https://res.cloudinary.com/dnpkvlbae/image/fetch/c_auto,f_auto,q_auto:good,w_800/https://s3.infra.brandquad.io/accounts-media/SHRP/DAM/origin/5aa3c3d6-4e73-11ef-b761-bea7e36404f4.jpg",
                    CategoryId = cat1.Id
                },
                new Product
                {
                    Name = "Alfa Romeo Giulietta Sport",
                    Description = "This Alfa Romeo Giulietta Sport is an Italian-made car with only one previous owner.\r\n\r\nAll documentation accompanying the vehicle is complete and available.\r\n\r\nRoutine maintenance has always been performed in accordance with Alfa Romeo guidelines, and the corresponding certification is available.\r\n\r\nThe car currently has 93,450 km on the odometer.\r\n\r\nThe engine and transmission are in perfect condition and would pass any diagnostic test. There are no noises to note or report.",
                    Price = 65000.00m,
                    ImageUrl = "https://autoacquistosicuro.com/wp-content/uploads/elementor/thumbs/IMG20230914102658-qceg4mjqsl60tqgloh86v2076j3ui113tnamctr52o.jpg",
                    CategoryId = cat3.Id
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            var reviews = new List<Review>
            {
                new Review
                {
                    ProductId = products[0].Id,
                    UserId = user1.Id,
                    Rating = 1,
                    Content = "Bad phone, sneezed and it bronke >:c \r\nWater bad for phon",
                    CreatedAt = DateTime.Now
                }
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            var cart = new Cart
            {
                UserId = user1.Id.ToString(),
                CreatedAt = DateTime.Now
            };

            context.Carts.Add(cart);
            context.SaveChanges();

            context.CartItems.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = products[0].Id,
                Quantity = 1
            });

            context.SaveChanges();

            var order = new Order
            {
                UserId = user1.Id,
                CreatedAt = DateTime.Now
            };

            context.Orders.Add(order);
            context.SaveChanges();

            context.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ProductId = products[0].Id,
                Quantity = 1
            });

            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
