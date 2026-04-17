using EGielda.Data;
using EGielda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGielda.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly EgieldaDbContext _context;

        public ReviewController(EgieldaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, int rating, string content)
        {
            if (rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Details", "Product", new { id = productId });
            }

            int userId = int.Parse(User.FindFirst("UserId")!.Value);

            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = rating,
                Content = content,
                CreatedAt = DateTime.Now
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}
