using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh230063_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }

        // Thong tin phan trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Danh sach 10 san pham noi bat
        public List<Product> FeaturedProducts { get; set; }

        // Danh sach 20 san pham moi co phan trang
        public IPagedList<Product> NewProducts { get; set; }
    }
}