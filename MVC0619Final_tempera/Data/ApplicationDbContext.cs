// Data/ApplicationDbContext.cs
namespace MVC0619Final_tempera.Data
{
    using Microsoft.EntityFrameworkCore;
    using MVC0619Final_tempera.Models; // 確保引入您的模型命名空間

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 定義 DbSet<Product>，如果您命名為 Item，這裡就是 DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; } // 注意：假設您已經將 DbSet<Product> Productss 更名為 DbSet<Product> Products

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 將 Product 模型映射到名為 "ProductTable" 的資料表
            modelBuilder.Entity<Product>().ToTable("ProductTable");

            base.OnModelCreating(modelBuilder); // 確保呼叫基類的 OnModelCreating
        }
    }
}