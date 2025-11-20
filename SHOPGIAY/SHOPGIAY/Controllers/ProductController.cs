using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOPGIAY.Data;

namespace SHOPGIAY.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShoeShopContext _context;

        public ProductController(ShoeShopContext context)
        {
            _context = context;
        }

        // /Product/Detail/5
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
