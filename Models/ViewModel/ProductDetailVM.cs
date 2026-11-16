using System.Collections.Generic;
using _25dh230063_MyStore.Models;

namespace _25dh230063_MyStore.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product product { get; set; }
        public int quantity { get; set; } = 1; // Số lượng mặc định là 1

        // Danh sách sản phẩm cùng loại
        public List<Product> RelatedProducts { get; set; }

        // Danh sách sản phẩm bán chạy cùng loại
        public List<Product> TopProducts { get; set; }
    }
}