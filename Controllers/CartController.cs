using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _25dh230063_MyStore.Models;

namespace _25dh230063_MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities(); // Tên DbContext của bạn

        // Hàm lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // 1. Hiển thị trang Giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // 2. Thêm sản phẩm vào giỏ (Dạng POST)
        [HttpPost]
        public ActionResult AddItem(int productId, int quantity)
        {
            var product = db.Products.Find(productId);
            if (product != null)
            {
                var cart = GetCart();
                // Kiểm tra xem sản phẩm này đã có trong giỏ chưa
                var item = cart.FirstOrDefault(c => c.ProductID == productId);

                if (item == null) // Nếu chưa có thì thêm mới
                {
                    cart.Add(new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        UnitPrice = product.ProductPrice,
                        Quantity = quantity
                    });
                }
                else // Nếu có rồi thì cộng dồn số lượng
                {
                    item.Quantity += quantity;
                }
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index"); // Thêm xong chuyển sang trang Giỏ hàng
        }

        // 3. Cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == productId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
            }
            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // 4. Xóa 1 sản phẩm khỏi giỏ
        [HttpPost]
        public ActionResult RemoveItem(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // 5. Xóa sạch giỏ hàng
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }
    }
}