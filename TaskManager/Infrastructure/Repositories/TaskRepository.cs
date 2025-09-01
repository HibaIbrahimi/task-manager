using Microsoft.EntityFrameworkCore;
using TaskManager.Domaine.Abstractions;
using TaskManager.Domaine.Entities;
using TaskManager.Infrastructure.DbContexts;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _db;

        public TaskRepository(TaskDbContext db) => _db = db;

        public async Task AddAsync(TaskItem entity, CancellationToken ct = default)
        {
            await _db.Tasks.AddAsync(entity, ct);
        }

        public Task DeleteAsync(TaskItem entity, CancellationToken ct = default)
        {
            _db.Tasks.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Tasks.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken ct = default)
        {
            return await _db.Tasks
            .AsNoTracking()
            .OrderBy(t => t.IsCompleted)
            .ToListAsync(ct);
        }

        public Task UpdateAsync(TaskItem entity, CancellationToken ct = default)
        {
            _db.Tasks.Update(entity);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}
