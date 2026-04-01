using EGielda.Data;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            ViewData["ShowSidebar"] = true;

            var products = _context.Products.Include(p => p.Category)
            .ToList();

            return View(products);
        }
    }
}
