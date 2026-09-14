using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Data;
using TaskFlowAPI.DTOs;
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
            var tasks = await _context.Tasks
                .Select(task => new TaskResponseDto
                {
                    Title = task.Title,
                    IsCompleted = task.IsCompleted
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            var response = new TaskResponseDto
            {
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                IsCompleted = dto.IsCompleted,
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Title = task.Title,
                IsCompleted = task.IsCompleted,
            };

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = task.Id},
                    response
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = dto.Title;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };

            return Ok(response);
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
