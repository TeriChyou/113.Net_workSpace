using Microsoft.EntityFrameworkCore; // 引入 EF Core 命名空間
using mvcPlayground.Data; // 引入您的 DbContext 命名空間

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen(); // Adds Swagger support

// Add services to the container.
builder.Services.AddControllersWithViews();

// 從配置中獲取連接字串
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 註冊您的 DbContext，並配置使用 SQL Server 提供者
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //db.Database.EnsureCreated(); // 這會直接創建資料庫，但不使用遷移
        dbContext.Database.Migrate(); // 這會應用所有未執行的遷移，推薦使用遷移
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


