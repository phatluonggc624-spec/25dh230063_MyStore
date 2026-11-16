using _25dh230063_MyStore.Models;
using _25dh230063_MyStore.Models.ViewModel; // Nhớ using cái này
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace _25dh230063_MyStore.Controllers
{
    public class HomeController : Controller
    {
        // Dùng đúng tên DbContext của bạn
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var products = db.Products.AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm));
            }

            // Lấy Top 10 sản phẩm bán chạy (đếm số lượng từ OrderDetail)
            var topProductIds = db.OrderDetails
                                  .GroupBy(od => od.ProductID)
                                  .OrderByDescending(g => g.Sum(x => x.Quantity))
                                  .Take(10)
                                  .Select(g => g.Key)
                                  .ToList();
            var featuredProducts = db.Products.Where(p => topProductIds.Contains(p.ProductID)).ToList();

            // Lấy 20 sản phẩm mới (phân trang 6 sản phẩm/trang)
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var newProducts = products.OrderByDescending(p => p.ProductID).ToPagedList(pageNumber, pageSize);

            // Truyền dữ liệu sang View
            HomeProductVM model = new HomeProductVM
            {
                SearchTerm = searchTerm,
                FeaturedProducts = featuredProducts,
                NewProducts = newProducts
            };

            return View(model);
        }
        // Action Xem chi tiết sản phẩm
        public ActionResult ProductDetail(int id)
        {
            // 1. Tìm sản phẩm theo ID
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound(); // Báo lỗi 404 nếu không tìm thấy
            }

            // 2. Lấy 8 sản phẩm cùng danh mục (trừ sản phẩm hiện tại ra)
            var relatedProducts = db.Products
                                    .Where(p => p.CategoryID == product.CategoryID && p.ProductID != id)
                                    .Take(8).ToList();

            // 3. Lấy Top 8 sản phẩm bán chạy cùng danh mục
            var topProductIds = db.OrderDetails
                                  .Where(od => od.Product.CategoryID == product.CategoryID)
                                  .GroupBy(od => od.ProductID)
                                  .OrderByDescending(g => g.Sum(x => x.Quantity))
                                  .Take(8)
                                  .Select(g => g.Key)
                                  .ToList();

            var topProducts = db.Products.Where(p => topProductIds.Contains(p.ProductID)).ToList();

            // 4. Đổ vào ViewModel
            ProductDetailVM model = new ProductDetailVM
            {
                product = product,
                RelatedProducts = relatedProducts,
                TopProducts = topProducts
            };

            return View(model);
        }
    }
}