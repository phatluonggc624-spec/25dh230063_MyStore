using System.ComponentModel.DataAnnotations;

namespace _25dh230063_MyStore.Models
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    public class CategoryMetadata
    {
        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên danh mục từ 2 đến 100 ký tự")]
        public string CategoryName { get; set; }
    }
}