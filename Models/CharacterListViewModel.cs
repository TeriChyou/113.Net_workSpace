// Models/CharacterListViewModel.cs
using System.Collections.Generic;

namespace mvcPlayground.Models
{
    public class CharacterListViewModel
    {
        // 目前頁面顯示的角色列表
        public List<Character>? Characters { get; set; }

        // 當前頁碼
        public int CurrentPage { get; set; }

        // 每頁顯示的項目數量
        public int PageSize { get; set; }

        // 資料總筆數
        public int TotalRecords { get; set; }

        // 總頁數
        public int TotalPages { get; set; }

        // (可選) 分頁導航列的 HTML 字串
        // 您可以選擇在這裡存放生成好的 HTML，或者在 View 中根據其他分頁資訊自己生成
        public string? PagingHtml { get; set; }
    }
}