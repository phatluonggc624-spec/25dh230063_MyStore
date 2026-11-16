using _25dh230063_MyStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace _25dh230063_MyStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index()
        {
            // Đếm số lượng
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalOrders = db.Orders.Count();
            
            // Tính tổng doanh thu (Nếu chưa có đơn nào thì trả về 0, tránh lỗi)
            ViewBag.TotalRevenue = db.Orders.Select(o => o.TotalAmount).DefaultIfEmpty(0).Sum();

            // Lấy 5 đơn hàng mới nhất để hiển thị ra bảng
            ViewBag.RecentOrders = db.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToList();

            return View();
        }
    }
}