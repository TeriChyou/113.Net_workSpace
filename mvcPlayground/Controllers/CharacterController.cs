using mvcPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using mvcPlayground.Data; // 引入您的 DbContext 所在的命名空間 (通常是 專案名稱.Data)
using Microsoft.EntityFrameworkCore; // 引入 Entity Framework Core 命名空間，以便使用異步方法 (例如 ToListAsync, SaveChangesAsync)
using System.Linq; // 確保包含這個命名空間，以便使用 LINQ 方法 (例如 FirstOrDefaultAsync)
using System.Threading.Tasks; // 引入 Task 命名空間，以便使用異步方法
using System.Text;

public class CharacterController : Controller
{
    // 移除之前靜態的 List<Character> Characters = ...;

    private readonly ApplicationDbContext _context; // 添加一個私有只讀字段來存放注入的 DbContext 實例

    // 構造函數：通過依賴注入 (Dependency Injection) 接收 ApplicationDbContext 實例
    public CharacterController(ApplicationDbContext context)
    {
        _context = context; // 將注入的 DbContext 賦值給私有字段
    }

    // GET: /Character/CharacterInfo
    // 新增 _ID 參數來接收當前頁碼，預設為 1
    // 您也可以新增一個 pageSize 參數來控制每頁顯示的數量，這裡暫時固定為 10
    public async Task<IActionResult> CharacterInfo(int _ID = 1) // _ID 代表目前位於第幾頁
    {
        // 設定每頁顯示的項目數量
        int PageSize = 10; // 您可以根據需要調整這個數字

        // 獲取資料總筆數
        int RecordCount = await _context.Characters.CountAsync(); // 從資料庫異步獲取總筆數

        // 計算總頁數
        // 使用 Math.Ceiling 來確保即使有餘數，也能額外分出一頁
        int TotalPages = (int)Math.Ceiling((double)RecordCount / PageSize);

        // 確保請求的頁碼在有效範圍內
        if (_ID < 1) _ID = 1;
        if (_ID > TotalPages && TotalPages > 0) _ID = TotalPages;
        else if (TotalPages == 0) _ID = 1; // 如果沒有數據，當前頁碼為 1

        // 計算要跳過的記錄數
        int SkipCount = (_ID - 1) * PageSize;

        // 從資料庫中獲取當前頁的數據
        var charactersOnPage = await _context.Characters
            .OrderBy(c => c.Id) // 請根據您的需求調整排序方式，分頁通常需要一個穩定的排序
            .Skip(SkipCount) // 跳過指定數量的記錄
            .Take(PageSize) // 取出指定數量的記錄 (每頁顯示數量)
            .ToListAsync(); // 異步轉換為列表

        // === 開始生成分頁導航列 HTML (參考您提供的程式碼邏輯) ===
        StringBuilder sbPageList = new StringBuilder();
        // 假設 Pages 是總頁數，_ID 是目前頁碼
        int Pages = TotalPages; // 將我們計算的總頁數賦值給 Pages 變數以匹配您提供的程式碼邏輯

        // 計算目前頁碼區塊 (每10頁為一個區塊)
        // 您提供的程式碼中 block_page 的計算方式有些特別，看起來是基於 0 開始的索引，且每 10 頁一塊
        // 讓我們稍微調整一下，確保和 _ID (從 1 開始) 對應
        int block_page = (_ID - 1) / 10; // 計算當前頁碼所在的區塊 (從 0 開始)

        // 產生「前十頁 <<」連結
        if (block_page > 0)
        {
             // 連接到前一個區塊的第一頁 (當前區塊起始頁 - 10)
             // 注意：需要處理負數情況，且確保跳轉到的頁碼大於等於 1
            int prevBlockStartPage = block_page * 10 - 9; // 前一個區塊的起始頁
            if (prevBlockStartPage < 1) prevBlockStartPage = 1;
            sbPageList.Append($"<a href=\"?_ID={prevBlockStartPage}\"> [前十頁<<] </a>&nbsp;&nbsp;");
        }

        // 產生頁碼連結
        // 迴圈生成當前區塊內的頁碼 (最多 10 個)
        // 這裡使用 K 從 0 到 9 來生成當前區塊的 10 個潛在頁碼連結
        for (int K = 0; K < 10; K++)
        {
            int currentPageNumber = block_page * 10 + K + 1; // 計算當前要生成的頁碼 (從 1 開始)

            // 確保生成的頁碼不超過總頁數
            if (currentPageNumber <= Pages)
            {
                if (currentPageNumber == _ID)
                {
                    // 當前頁碼加粗顯示
                    sbPageList.Append($"[<b>{_ID}</b>]&nbsp;&nbsp;&nbsp;");
                }
                else
                {
                    // 其他頁碼生成連結
                     sbPageList.Append($"<a href=\"?_ID={currentPageNumber}\">{currentPageNumber}</a>");
                     sbPageList.Append("&nbsp;&nbsp;&nbsp;");
                }
            }
        }

        // 產生「後十頁 >>」連結
        // 計算下一個區塊的第一頁
        int nextBlockStartPage = (block_page + 1) * 10 + 1;

        // 如果下一個區塊的第一頁小於等於總頁數，則顯示「後十頁 >>」連結
        if (nextBlockStartPage <= Pages)
        {
             // 跳轉到下一個區塊的第一頁
             sbPageList.Append($"&nbsp;&nbsp;<a href=\"?_ID={nextBlockStartPage}\"> [後十頁>>] </a>");
        }

        // 您可能還想添加「首頁」和「末頁」的連結
        // 首頁連結
        if (_ID > 1)
        {
             // sbPageList.Insert(0, $"<a href=\"?_ID=1\">[首頁]</a>&nbsp;&nbsp;"); // 插入到開頭
        }
        // 末頁連結
        if (_ID < TotalPages)
        {
             // sbPageList.Append($"&nbsp;&nbsp;<a href=\"?_ID={TotalPages}\">[末頁]</a>");
        }

        // === 生成分頁導航列 HTML 結束 ===


        // 創建 ViewModel 實例並填充數據
        var viewModel = new CharacterListViewModel
        {
            Characters = charactersOnPage, // 當前頁的角色列表
            CurrentPage = _ID, // 當前頁碼
            PageSize = PageSize, // 每頁顯示數量
            TotalRecords = RecordCount, // 總記錄數
            TotalPages = TotalPages, // 總頁數
            PagingHtml = sbPageList.ToString() // 生成的分頁導航列 HTML
        };

        // 將 ViewModel 傳遞給 View
        return View(viewModel);
    }

    // GET: /Character/Details/5
    // 顯示單個角色的詳細資訊
    // id 參數可以為 null，用於處理無效的請求路徑
    public async Task<IActionResult> Details(int? id) // Action 方法改為異步
    {
        if (id == null)
        {
            return NotFound(); // 如果沒有提供 Id，返回 404 Not Found
        }

        // 使用 _context.Characters 從資料庫中異步查找指定 Id 的角色
        // FirstOrDefaultAsync 會返回第一個匹配項或預設值 (對於引用類型是 null)
        var character = await _context.Characters
            .FirstOrDefaultAsync(m => m.Id == id);

        if (character == null)
        {
            return NotFound(); // 如果找不到指定 Id 的角色，返回 404 Not Found
        }

        return View(character); // 將找到的角色對象傳遞給 View
    }

    // GET: /Character/Create
    // 顯示新增角色的表單
    public IActionResult Create()
    {
        return View(); // 直接返回 Create View，該 View 會顯示一個表單
    }

    // POST: /Character/Create
    // 處理新增角色的表單提交
    [HttpPost] // 標明這個 Action 方法只處理 POST 請求
    [ValidateAntiForgeryToken] // 驗證 Anti-Forgery Token，防止 CSRF 攻擊
    // Bind 屬性用於限制模型繫結時只包含 Name, Job, Level 屬性，提高安全性
    public async Task<IActionResult> Create([Bind("Name,Job,Level")] Character character) // Action 方法改為異步
    {
        if (ModelState.IsValid) // 檢查通過模型驗證 (例如，如果您的 Model 中有 [Required] 屬性)
        {
            _context.Add(character); // 將新的 Character 對象添加到 DbContext 的追蹤中
            await _context.SaveChangesAsync(); // 異步保存更改到資料庫 (執行 INSERT 語句)
            return RedirectToAction(nameof(CharacterInfo)); // 重定向回角色列表頁面
        }
        return View(character); // 如果模型驗證失敗，返回 Create View 並顯示驗證錯誤
    }

    // GET: /Character/Edit/5
    // 顯示編輯角色的表單
    public async Task<IActionResult> Edit(int? id) // Action 方法改為異步
    {
        if (id == null)
        {
            return NotFound();
        }

        // 使用 FindAsync 異步根據主鍵查找角色
        var character = await _context.Characters.FindAsync(id);
        if (character == null)
        {
            return NotFound();
        }
        return View(character); // 將找到的角色對象傳遞給 Edit View (表單會預填現有資料)
    }

    // POST: /Character/Edit/5
    // 處理編輯角色的表單提交
    [HttpPost]
    [ValidateAntiForgeryToken]
    // 確保 Id 屬性也在綁定範圍內，以便在 POST 請求中獲取要更新的角色的 Id
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Job,Level")] Character character) // Action 方法改為異步
    {
        // 檢查傳入的 id 是否與模型中的 Id 一致
        if (id != character.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(character); // 將 Character 對象標記為已修改
                await _context.SaveChangesAsync(); // 異步保存更改到資料庫 (執行 UPDATE 語句)
            }
            catch (DbUpdateConcurrencyException) // 處理併發更新時可能發生的異常
            {
                if (!CharacterExists(character.Id)) // 如果角色不存在，返回 404
                {
                    return NotFound();
                }
                else
                {
                    throw; // 拋出其他類型的併發異常
                }
            }
            return RedirectToAction(nameof(CharacterInfo)); // 重定向回列表頁面
        }
        return View(character); // 如果模型驗證失敗，返回 Edit View
    }

    // GET: /Character/Delete/5
    // 顯示刪除確認頁面
    public async Task<IActionResult> Delete(int? id) // Action 方法改為異步
    {
        if (id == null)
            {
                return NotFound();
            }

        // 異步查找要刪除的角色，並包含可能的關聯實體 (雖然目前沒有)
        var character = await _context.Characters
            // .Include(c => c.RelatedEntities) // 如果有相關聯的實體，可以在此 Include
            .FirstOrDefaultAsync(m => m.Id == id);

        if (character == null)
        {
            return NotFound();
        }

        return View(character); // 將找到的角色對象傳遞給 Delete View (顯示要刪除的角色資訊)
    }

    // POST: /Character/Delete/5
    // 處理刪除操作
    [HttpPost, ActionName("Delete")] // 指定這個 POST 方法的名字為 "Delete"，用於匹配路由
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) // Action 方法改為異步，接收要刪除的角色 Id
    {
        // 異步查找要刪除的角色
        var character = await _context.Characters.FindAsync(id);

        if (character != null)
        {
            _context.Characters.Remove(character); // 從 DbContext 中移除角色
            await _context.SaveChangesAsync(); // 異步保存更改到資料庫 (執行 DELETE 語句)
        }

        return RedirectToAction(nameof(CharacterInfo)); // 重定向回列表頁面
    }

    
    // 幫助方法：檢查指定 Id 的角色是否存在於資料庫中
    private bool CharacterExists(int id)
    {
        // Any() 方法會檢查 DbSet 中是否存在滿足條件的實體。
        // GetValueOrDefault() 用於處理 _context.Characters 可能為 null 的情況 (通常不會發生，但是一種防禦性編程)
        return (_context.Characters?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}