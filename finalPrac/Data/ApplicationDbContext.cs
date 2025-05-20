// Data/ApplicationDbContext.cs

// 明確聲明命名空間，這點很重要，可以解決之前遇到的命名空間錯誤
namespace finalPrac.Data
{
    using Microsoft.EntityFrameworkCore;
    using finalPrac.Models; // 確保引入您的 Character 模型所在的命名空間

    public class ApplicationDbContext : DbContext
    {
        // 構造函數，用於接收配置選項
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Productss { get; set; }
    }
}