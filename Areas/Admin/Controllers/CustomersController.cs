using _25dh230063_MyStore.Areas.Admin.Filters;
using _25dh230063_MyStore.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace _25dh230063_MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Hàm con: nạp danh sách Username cho DropDownList (Create, Edit)
        private void LoadUsers(string selected = null)
        {
            ViewBag.Username = new SelectList(db.Users, "Username", "Username", selected);
        }

        // Hàm con dùng chung: kiểm tra id, tìm khách hàng (Details, Edit GET, Delete GET)
        private ActionResult GetCustomerView(int? id, bool loadUsers = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            if (loadUsers) LoadUsers(customer.Username);
            return View(customer);
        }

        // GET: Admin/Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.User);
            return View(customers.ToList());
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            return GetCustomerView(id);
        }

        // GET: Admin/Customers/Create
        public ActionResult Create()
        {
            LoadUsers();
            return View();
        }

        // POST: Admin/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,Username")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadUsers(customer.Username);
            return View(customer);
        }

        // GET: Admin/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            return GetCustomerView(id, true);
        }

        // POST: Admin/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,Username")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadUsers(customer.Username);
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            return GetCustomerView(id);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}