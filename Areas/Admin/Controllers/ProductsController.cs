using _25dh230063_MyStore.Areas.Admin.Filters;
using _25dh230063_MyStore.Models;
using _25dh230063_MyStore.Models.ViewModel;
using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace _25dh230063_MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Hàm con dùng chung: kiểm tra id, tìm sản phẩm (dùng cho Details, Edit GET, Delete GET)
        private ActionResult GetProductView(int? id, bool loadCategories = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (loadCategories)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            }
            return View(product);
        }

        // Hàm con dùng chung: kiểm tra đuôi file và lưu ảnh upload vào Content/images
        private bool SaveUploadedImage(Product product)
        {
            if (product.UploadImg != null && product.UploadImg.ContentLength > 0)
            {
                string ext = Path.GetExtension(product.UploadImg.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif")
                {
                    ModelState.AddModelError("UploadImg", "Chỉ chấp nhận các định dạng ảnh PNG, JPG và GIF");
                    return false;
                }
                string filename = Path.GetFileName(product.UploadImg.FileName);
                string savePath = "~/Content/images/";

                // Chỉ lưu tên file vào DB
                product.ProductImage = filename;

                // Lưu file vật lý vào thư mục
                product.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
            }
            return true;
        } // <--- Đã thêm dấu ngoặc nhọn bị thiếu ở đây

        // GET: Admin/Products
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.Include(p => p.Category).AsQueryable();

            // Tìm sản phẩm dựa trên từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                model.SearchTerm = searchTerm;
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    (p.ProductDescription != null && p.ProductDescription.Contains(searchTerm)) ||
                    p.Category.CategoryName.Contains(searchTerm));
            }

            // Tìm theo giá tối thiểu
            if (minPrice.HasValue)
            {
                model.MinPrice = minPrice;
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }

            // Tìm theo giá tối đa
            if (maxPrice.HasValue)
            {
                model.MaxPrice = maxPrice;
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }

            // Sắp xếp
            model.SortOrder = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                default: // name_asc hoặc mặc định
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            // Phân trang
            int pageNumber = page ?? 1;
            int pageSize = 2;  // Số sản phẩm mỗi trang

            model.PageNumber = pageNumber;
            model.PageSize = pageSize;
            model.Products = products.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            return GetProductView(id);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription," +
            "ProductPrice,ProductImage,UploadImg")] Product product)
        {
            if (ModelState.IsValid && SaveUploadedImage(product))
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            return GetProductView(id, true);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription," +
            "ProductPrice,ProductImage,UploadImg")] Product product)
        {
            if (ModelState.IsValid && SaveUploadedImage(product))
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            return GetProductView(id);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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