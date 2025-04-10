using mvcPlayground.Models;
using Microsoft.AspNetCore.Mvc;
public class CharacterController : Controller
{
    // Default data
    private static List<Character> Characters = new List<Character>
    {
        new Character { Id = 1, Name = "John", Job = "Priest", Level = 29 },
        new Character { Id = 2, Name = "Jack", Job = "Rogue", Level = 15 },
        new Character { Id = 3, Name = "Joseph", Job = "Beginner", Level = 3 },
        new Character { Id = 4, Name = "Jose", Job = "Beginner", Level = 9 },
        new Character { Id = 5, Name = "Josh", Job = "Nobody", Level = 1}
    };

    public IActionResult CharacterInfo()
    {
        return View(Characters);
    }

    public IActionResult Details(int id)
    {
        var Character = Characters.FirstOrDefault(p => p.Id == id);
        if (Character == null) return NotFound();
        return View(Character);
    }
}
