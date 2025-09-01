using TaskManager.Domaine.Entities;

namespace TaskManager.Domaine.Abstractions
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem entity, CancellationToken ct = default);
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct = default);
        Task UpdateAsync(TaskItem entity, CancellationToken ct = default);
        Task DeleteAsync(TaskItem entity, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
