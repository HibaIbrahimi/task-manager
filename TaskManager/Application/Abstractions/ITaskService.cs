using TaskManager.Application.DTOs;

namespace TaskManager.Application.Abstractions
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(Guid id);
        Task<TaskDto> CreateAsync(CreateTaskCommand command);
        Task<bool> CompleteAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
