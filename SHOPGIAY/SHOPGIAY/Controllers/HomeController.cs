using Microsoft.AspNetCore.Mvc;
using SHOPGIAY.Data;

namespace SHOPGIAY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoeShopContext _context;

        public HomeController(ShoeShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}
