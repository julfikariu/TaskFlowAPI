using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Data;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _context.Tasks.ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title;
            task.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
