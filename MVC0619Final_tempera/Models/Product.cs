// Models/Item.cs (或者 Models/Product.cs，請保持一致)
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 用於 [Table] 註解

namespace MVC0619Final_tempera.Models // <-- 確保命名空間與您的專案名稱一致
{
    [Table("ProductTable")]
    public class Product
    {
        // [Key] // ProductID will be recognized as primary key by convention, but you can explicitly add it
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")] // Make ProductName required
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")] // Nvarchar(100)
        [Display(Name = "Product Name")] // Custom display name for UI
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Category is required.")]
        [StringLength(50, ErrorMessage = "Product Category cannot exceed 50 characters.")] // Nvarchar(50)
        [Display(Name = "Category")]
        public string ProductCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Company is required.")]
        [StringLength(50, ErrorMessage = "Product Company cannot exceed 50 characters.")] // Nvarchar(50)
        [Display(Name = "Company")]
        public string ProductCompany { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Date is required.")]
        [DataType(DataType.Date)] // Specify this as a Date type for UI helpers
        [Display(Name = "Date Added")]
        public DateTime ProductDate { get; set; }

        [Required(ErrorMessage = "Product Price is required.")]
        [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000.")] // Example range
        [Display(Name = "Price")]
        public int ProductPrice { get; set; }
    }
}