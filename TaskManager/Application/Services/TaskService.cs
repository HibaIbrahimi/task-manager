using FluentValidation;
using TaskManager.Application.Abstractions;
using TaskManager.Application.DTOs;
using TaskManager.Domaine.Abstractions;
using TaskManager.Domaine.Entities;
using TaskManager.Domaine.ValueObjects;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<TaskItem> _validator;

        public TaskService(ITaskRepository repository, IValidator<TaskItem> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _repository.ListAsync();
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title.Value,
                IsCompleted = t.IsCompleted
            });
        }

        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title.Value,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskDto> CreateAsync(CreateTaskCommand command)
        {
            var task = new TaskItem(new TaskTitle(command.Title));

            var validationResult = _validator.Validate(task);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title.Value,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<bool> CompleteAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            task.Complete();
            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            await _repository.DeleteAsync(task);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
