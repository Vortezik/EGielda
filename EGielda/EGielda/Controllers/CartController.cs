using EGielda.Data;
using EGielda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGielda.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly EgieldaDbContext _context;

        public CartController(EgieldaDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirst("UserId")!.Value;
        }

        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    CartItems = new List<CartItem>()
                };
            }
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            string userId = GetUserId();

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
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    _context.CartItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            string userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = int.Parse(userId),
                CreatedAt = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Success", new { id = order.Id });
        }

        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
