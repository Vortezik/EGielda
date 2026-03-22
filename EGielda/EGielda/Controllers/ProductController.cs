using Microsoft.AspNetCore.Mvc;

namespace EGielda.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ShowSidebar"] = true;
            return View();
        }
    }
}
