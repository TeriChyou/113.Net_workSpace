using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TaskAPIController : ControllerBase
{
    // Local listed task.
    private static List<string> tasks = new List<string> { "Task 1", "Task 2" };
    

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        if (id < 0 || id >= tasks.Count)
            return NotFound(new { message = "Task not found!" });

        return Ok(new { id, task = tasks[id] });
    }

    [HttpPost]
    public IActionResult Create([FromBody] string newTask)
    {
        tasks.Add(newTask);
        return Created("", new { message = "Task added!", task = newTask });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] string updatedTask)
    {
        if (id < 0 || id >= tasks.Count)
            return NotFound(new { message = "Task not found!" });

        tasks[id] = updatedTask;
        return Ok(new { message = "Task updated!", task = updatedTask });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (id < 0 || id >= tasks.Count)
            return NotFound(new { message = "Task not found!" });

        tasks.RemoveAt(id);
        return Ok(new { message = "Task deleted!" });
    }
}
