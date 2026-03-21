using Microsoft.AspNetCore.Mvc;

namespace EGielda.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
