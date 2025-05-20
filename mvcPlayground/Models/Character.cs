// Models/Characterl.cs
namespace mvcPlayground.Models;
public class Character
{
    required public int Id { get; set; }
    required public string Name { get; set; }
    required public string Job { get; set; }
    required public int Level { get; set; }
}
