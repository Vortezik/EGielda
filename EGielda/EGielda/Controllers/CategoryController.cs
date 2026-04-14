using EGielda.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGielda.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EgieldaDbContext _context;

        public CategoryController(EgieldaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
