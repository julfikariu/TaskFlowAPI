using TaskFlowAPI.DTOs;

namespace TaskFlowAPI.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetAllAsync();

        Task<TaskResponseDto?> GetByIdAsync(int id);

        Task<List<TaskResponseDto>?> GetTasksByProjectAsync(int projectId);

        Task<TaskResponseDto?> CreateAsync(CreateTaskDto task);

        Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto task);

        Task<bool> DeleteAsync(int id);
    }
}
