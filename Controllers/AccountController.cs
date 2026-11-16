using System.Linq;
using System.Web.Mvc;
using _25dh230063_MyStore.Models;
using _25dh230063_MyStore.Models.ViewModel;

namespace _25dh230063_MyStore.Controllers
{
    public class AccountController : Controller
    {
        // Đã dùng đúng tên DbContext của bạn
        private MyStoreEntities db = new MyStoreEntities();

        // --- ĐĂNG NHẬP ---
        public ActionResult Login() { return View(); }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null && user.UserRole == "C")
            {
                var customer = db.Customers.FirstOrDefault(c => c.Username == username);
                if (customer != null)
                {
                    Session["CustomerID"] = customer.CustomerID;
                    Session["CustomerName"] = customer.CustomerName;
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // --- ĐĂNG KÝ ---
        public ActionResult Register() { return View(); }

        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản đã tồn tại chưa
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                    return View(model);
                }

                // 1. Lưu bảng User
                var newUser = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "C" // C là Khách hàng
                };
                db.Users.Add(newUser);

                // 2. Lưu bảng Customer
                var newCustomer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerPhone,
                    CustomerEmail = model.CustomerEmail,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customers.Add(newCustomer);

                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // --- ĐĂNG XUẤT ---
        public ActionResult Logout()
        {
            Session["CustomerID"] = null;
            Session["CustomerName"] = null;
            Session.Clear(); // Xóa sạch để an toàn
            return RedirectToAction("Index", "Home");
        }
    }
}