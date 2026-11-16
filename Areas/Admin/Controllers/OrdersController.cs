using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _25dh230063_MyStore.Models;

namespace _25dh230063_MyStore.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. Danh sách tất cả đơn hàng
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 2. Xem chi tiết 1 đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.Orders.Include(o => o.Customer).FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            var orderDetails = db.OrderDetails.Include(od => od.Product).Where(od => od.OrderID == id).ToList();

            ViewBag.OrderInfo = order;
            return View(orderDetails);
        }
    }
}