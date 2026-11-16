using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh230063_MyStore.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // Tự động tính thành tiền (Giá * Số lượng)
        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }
    }
}