using System;
using System.Collections.Generic;
using System.Web; // Cần cái này để dùng HttpPostedFileBase
using System.ComponentModel.DataAnnotations.Schema; // Cần cái này để dùng [NotMapped]

namespace _25dh230063_MyStore.Models
{
    public partial class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }

        // THÊM DÒNG NÀY VÀO: Dùng để hứng file ảnh upload
        [NotMapped] // Báo cho Entity Framework biết không lưu trường này xuống Database
        public HttpPostedFileBase UploadImg { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}