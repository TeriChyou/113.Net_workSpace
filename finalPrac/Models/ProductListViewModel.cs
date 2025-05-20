// Models/ProductListViewModel.cs
using System.Collections.Generic;

namespace finalPrac.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public string PagingHtml { get; set; }
    }
}