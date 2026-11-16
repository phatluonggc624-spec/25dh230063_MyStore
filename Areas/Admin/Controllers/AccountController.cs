using System.Linq;
using System.Web.Mvc;
using _25dh230063_MyStore.Models;
using _25dh230063_MyStore.Models.ViewModel; // Cần dùng RegisterVM

namespace _25dh230063_MyStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // --- ĐĂNG NHẬP ADMIN ---
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.UserRole == "A");

            if (user != null)
            {
                Session["AdminUser"] = user.Username;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            ViewBag.Error = "Sai tài khoản hoặc không có quyền Admin!";
            return View();
        }

        // --- ĐĂNG KÝ ADMIN (MỚI) ---
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra Username tồn tại chưa
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng!");
                    return View(model);
                }

                // Chỉ lưu vào bảng User với quyền 'A' (Admin)
                var newAdmin = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "A"
                };
                db.Users.Add(newAdmin);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // --- ĐĂNG XUẤT ADMIN ---
        public ActionResult Logout()
        {
            Session["AdminUser"] = null;
            Session.Clear();
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
    }
}