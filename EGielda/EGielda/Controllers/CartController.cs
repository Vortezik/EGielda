using EGielda.Data;
using EGielda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGielda.Controllers
{
    public class CartController : Controller
    {
        private readonly EgieldaDbContext _context;

        public CartController(EgieldaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            string userId = User.FindFirst("UserId")?.Value!;

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (item == null)
            {
                item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.CartItems.Add(item);
            }
            else
            {
                item.Quantity++;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}
