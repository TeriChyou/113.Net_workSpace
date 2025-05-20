// Data/ApplicationDbContext.cs

// 明確聲明命名空間
namespace mvcPlayground.Data
{
    using Microsoft.EntityFrameworkCore;
    using mvcPlayground.Models; // 確保引入您的 Model 命名空間

    public class ApplicationDbContext : DbContext
    {
        // 構造函數，用於接收配置選項
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 為 Character 模型創建 DbSet。
        public DbSet<Character> Characters { get; set; }

        // 如果您有其他需要存儲在資料庫中的模型，也在這裡添加 DbSet
        // public DbSet<Task> Tasks { get; set; }

        // 您可以在此選擇性地覆寫 OnModelCreating 方法
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}

