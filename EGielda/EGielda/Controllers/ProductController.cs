using EGielda.Data;
using EGielda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EGielda.Controllers
{
    public class ProductController : Controller
    {
        private readonly EgieldaDbContext _context;

        public ProductController(EgieldaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string search)
        {
            ViewData["ShowSidebar"] = true;

            ViewBag.Categories = await _context.Categories.ToListAsync();

            var products = _context.Products.Include(p => p.Category)
            .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId);

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice);

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
    }
}
