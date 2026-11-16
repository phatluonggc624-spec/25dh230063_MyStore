using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace _25dh230063_MyStore.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    { 
    }

    public class ProductMetadata
    {
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Tên sản phẩm từ 2 đến 200 ký tự")]
        public string ProductName { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự")]
        public string ProductDescription { get; set; }

        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(0, 1000000000, ErrorMessage = "Đơn giá phải từ 0 đến 1.000.000.000")]
        public decimal ProductPrice { get; set; }
    }
}