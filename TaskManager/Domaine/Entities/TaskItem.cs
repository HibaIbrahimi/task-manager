using Microsoft.CodeAnalysis.CSharp.Syntax;
using TaskManager.Domaine.ValueObjects;

namespace TaskManager.Domaine.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public TaskTitle Title { get; private set; }
        public bool IsCompleted { get; private set; }


        public TaskItem() { }


        public TaskItem(TaskTitle title)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            IsCompleted = false;
        }


        public void Rename(TaskTitle newTitle) => Title = newTitle ?? throw new ArgumentNullException(nameof(newTitle));
        public void Complete()
        {
            if (IsCompleted)
                throw new InvalidOperationException("Task already completed.");

            IsCompleted = true;
        }
        public void Reopen() => IsCompleted = false;
    }
}
