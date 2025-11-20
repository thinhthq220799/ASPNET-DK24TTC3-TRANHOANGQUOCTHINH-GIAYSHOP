using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SHOPGIAY.Data;
using SHOPGIAY.Models;

namespace SHOPGIAY.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoeShopContext _context;

        public CartController(ShoeShopContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("CART");
            if (string.IsNullOrEmpty(sessionData))
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(sessionData)
                   ?? new List<CartItem>();
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("CART", json);
        }

        // ================== GIỎ HÀNG ==================

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // ================== THÊM / MUA NGAY ==================

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Thumbnail = product.Thumbnail ?? "/images/default.jpg",
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity += 1;
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult BuyNow(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            var cart = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Thumbnail = product.Thumbnail ?? "/images/default.jpg",
                    Quantity = 1
                }
            };

            SaveCart(cart);
            return RedirectToAction("Checkout");
        }

        // ================== TĂNG / GIẢM / XÓA ==================

        public IActionResult Increase(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity += 1;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity -= 1;
                if (item.Quantity <= 0)
                    cart.Remove(item);

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // ================== CHECKOUT ==================

        // GET: /Cart/Checkout
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                // Không có gì trong giỏ → quay lại giỏ
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // POST: /Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(
            string customerName,
            string customerPhone,
            string customerEmail,
            string shippingAddress,
            string? note)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(customerName) ||
                string.IsNullOrWhiteSpace(customerPhone) ||
                string.IsNullOrWhiteSpace(shippingAddress))
            {
                TempData["CheckoutError"] = "Vui lòng nhập đầy đủ Họ tên, Số điện thoại và Địa chỉ giao hàng.";
                return View(cart);
            }

            // Tính tổng tiền
            decimal totalAmount = cart.Sum(x => x.Price * x.Quantity);

            // Tạo Order
            var order = new Order
            {
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                CustomerEmail = string.IsNullOrWhiteSpace(customerEmail) ? null : customerEmail,
                ShippingAddress = shippingAddress,
                OrderDate = DateTime.Now,
                Status = "New",
                TotalAmount = totalAmount,
                Note = note
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // để có OrderId

            // Tạo OrderDetails
            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    Discount = 0,
                    LineTotal = item.Price * item.Quantity
                };

                _context.OrderDetails.Add(detail);

                // Trừ tồn kho (nếu có dùng)
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                }
            }

            _context.SaveChanges();

            // Xóa giỏ hàng
            HttpContext.Session.Remove("CART");

            // Chuyển sang trang cảm ơn
            return RedirectToAction("Success", new { id = order.OrderId });
        }

        // ================== ORDER SUCCESS ==================

        public IActionResult Success(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null) return NotFound();

            return View(order);
        }
    }
}
