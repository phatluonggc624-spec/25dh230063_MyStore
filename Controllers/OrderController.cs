using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _25dh230063_MyStore.Models;

namespace _25dh230063_MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. Trang Thanh toán (GET)
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart"); // Giỏ trống thì về lại giỏ
            }

            // Bắt buộc phải đăng nhập mới được thanh toán
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(cart);
        }

        // 2. Xử lý Đặt hàng (POST)
        [HttpPost]
        public ActionResult Checkout(string addressDelivery)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Tạo Đơn hàng mới
            var order = new Order
            {
                CustomerID = (int)Session["CustomerID"],
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(x => x.TotalPrice),
                PaymentStatus = "Chưa thanh toán",
                AddressDelivery = addressDelivery
            };
            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu Chi tiết đơn hàng
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            // Xóa giỏ hàng sau khi đặt thành công
            Session["Cart"] = null;

            return RedirectToAction("OrderSuccess", new { id = order.OrderID });
        }

        // 3. Trang Đặt hàng thành công
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Find(id);
            return View(order);
        }

        // 4. Trang Lịch sử mua hàng
        public ActionResult OrderHistory()
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int customerId = (int)Session["CustomerID"];
            var orders = db.Orders.Where(o => o.CustomerID == customerId).OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }
    }
}