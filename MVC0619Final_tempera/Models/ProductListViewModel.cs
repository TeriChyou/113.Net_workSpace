// Models/ProductListViewModel.cs
using System.Collections.Generic;

namespace MVC0619Final_tempera.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public string PagingHtml { get; set; }

        // --- 新增的屬性 ---
        public string? SearchTerm { get; set; } // 用於儲存目前的搜尋關鍵字
        public string? SortBy { get; set; }     // 用於儲存目前的排序選項 (例如 "priceAsc", "companyDesc")
        // --- 新增的屬性結束 ---
    }
}