using MVC0619Final_tempera.Data;
using MVC0619Final_tempera.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization; // 引入此命名空間

// 移除類別層級的 [Authorize] 屬性，讓未登入用戶也能訪問此控制器下的 Action，除非特別指定
// [Authorize] // <-- 移除或註解掉這一行
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 允許未登入使用者和所有已登入使用者訪問產品列表頁面
    [AllowAnonymous]
    public async Task<IActionResult> Index(
        int _ID = 1,
        string? searchTerm = null,
        string? sortBy = null
    )
    {
        // ... (保持 Index Action 方法的其餘程式碼不變) ...
        int PageSize = 10;
        IQueryable<Product> productsQuery = _context.Products; // Using Products now

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            productsQuery = productsQuery.Where(p => p.ProductName.Contains(searchTerm));
        }

        switch (sortBy)
        {
            case "priceAsc":
                productsQuery = productsQuery.OrderBy(p => p.ProductPrice);
                break;
            case "priceDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.ProductPrice);
                break;
            case "companyAsc":
                productsQuery = productsQuery.OrderBy(p => p.ProductCompany);
                break;
            case "companyDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.ProductCompany);
                break;
            case "categoryAsc":
                productsQuery = productsQuery.OrderBy(p => p.ProductCategory);
                break;
            case "categoryDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.ProductCategory);
                break;
            case "nameAsc":
                productsQuery = productsQuery.OrderBy(p => p.ProductName);
                break;
            case "nameDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.ProductName);
                break;
            default:
                productsQuery = productsQuery.OrderBy(p => p.ProductID);
                break;
        }

        int RecordCount = await productsQuery.CountAsync();
        int TotalPages = (int)Math.Ceiling((double)RecordCount / PageSize);

        if (_ID < 1) _ID = 1;
        if (_ID > TotalPages && TotalPages > 0) _ID = TotalPages;
        else if (TotalPages == 0) _ID = 1;

        int SkipCount = (_ID - 1) * PageSize;

        var productsOnPage = await productsQuery
            .Skip(SkipCount)
            .Take(PageSize)
            .ToListAsync();

        StringBuilder sbPageList = new StringBuilder();

        sbPageList.Append("<ul class=\"pagination justify-content-center\">");

        string CreatePageUrl(int pageNumber)
        {
            return Url.Action("Index", new { _ID = pageNumber, searchTerm = searchTerm, sortBy = sortBy });
        }

        string prevDisabled = (_ID == 1 || TotalPages == 0) ? "disabled" : "";
        sbPageList.Append($"<li class=\"page-item {prevDisabled}\">");
        sbPageList.Append($"<a class=\"page-link\" href=\"{CreatePageUrl(_ID - 1)}\" aria-label=\"Previous\">");
        sbPageList.Append("<span aria-hidden=\"true\">&laquo;</span>");
        sbPageList.Append("<span class=\"sr-only\">Previous</span>");
        sbPageList.Append("</a></li>");

        int maxPagesToShow = 5;
        int startPage = Math.Max(1, _ID - (maxPagesToShow / 2));
        int endPage = Math.Min(TotalPages, startPage + maxPagesToShow - 1);

        if (endPage - startPage + 1 < maxPagesToShow)
        {
            startPage = Math.Max(1, endPage - maxPagesToShow + 1);
        }

        for (int i = startPage; i <= endPage; i++)
        {
            string activeClass = (i == _ID) ? "active" : "";
            sbPageList.Append($"<li class=\"page-item {activeClass}\">");
            sbPageList.Append($"<a class=\"page-link\" href=\"{CreatePageUrl(i)}\">{i}</a>");
            sbPageList.Append("</li>");
        }

        string nextDisabled = (_ID == TotalPages || TotalPages == 0) ? "disabled" : "";
        sbPageList.Append($"<li class=\"page-item {nextDisabled}\">");
        sbPageList.Append($"<a class=\"page-link\" href=\"{CreatePageUrl(_ID + 1)}\" aria-label=\"Next\">");
        sbPageList.Append("<span aria-hidden=\"true\">&raquo;</span>");
        sbPageList.Append("<span class=\"sr-only\">Next</span>");
        sbPageList.Append("</a></li>");

        sbPageList.Append("</ul>");

        var viewModel = new ProductListViewModel
        {
            Products = productsOnPage,
            CurrentPage = _ID,
            PageSize = PageSize,
            TotalRecords = RecordCount,
            TotalPages = TotalPages,
            PagingHtml = sbPageList.ToString(),
            SearchTerm = searchTerm,
            SortBy = sortBy
        };

        return View(viewModel);
    }

    // 允許未登入使用者和所有已登入使用者訪問產品詳細資訊頁面
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
        if (product == null) return NotFound();
        return View(product);
    }

    // 只有 Admin 角色可以創建
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("ProductName,ProductCategory,ProductCompany,ProductDate,ProductPrice")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // 只有 Admin 角色可以編輯
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductCategory,ProductCompany,ProductDate,ProductPrice")] Product product)
    {
        if (id != product.ProductID) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductID)) { return NotFound(); }
                else { throw; }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // 只有 Admin 角色可以刪除
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
    }
}