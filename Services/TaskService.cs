using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Data;
using TaskFlowAPI.DTOs;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskResponseDto>> GetAllAsync()
        {
            return await _context.Tasks
                .Select(task => new TaskResponseDto
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    ProjectName = task.Project.Name,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted
                })
                .ToListAsync();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
             return await _context.Tasks
                .Where(task => task.Id == id)
                .Select(task => new TaskResponseDto
                {
                     Id = task.Id,
                     ProjectId = task.ProjectId,
                     Title = task.Title,
                     Description = task.Description,
                     IsCompleted = task.IsCompleted
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<TaskResponseDto>?> GetTasksByProjectAsync(int projectId)
        {
            var projectExists = await _context.Projects
                .AnyAsync(project => project.Id == projectId);

            if (!projectExists)
            {
                return null; 
            }

            var tasks = await _context.Tasks
                .Where(task => task.ProjectId == projectId)
                .Select(task => new TaskResponseDto
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted
                })
                .ToListAsync();
            return tasks;
        }

        public async Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto)
        {
            var projectExists = await _context.Projects
                .AnyAsync(project => project.Id == dto.ProjectId);

            if (!projectExists)
            {
                return null;
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                ProjectId = dto.ProjectId,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                ProjectId = task.ProjectId,
                Description = dto.Description,
                IsCompleted = task.IsCompleted,
            };
        }

        public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return null;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
