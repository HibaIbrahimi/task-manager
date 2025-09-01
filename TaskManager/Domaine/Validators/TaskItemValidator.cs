using FluentValidation;
using TaskManager.Domaine.Entities;

namespace TaskManager.Domaine.Validators
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .SetValidator(new TaskTitleValidator());

            RuleFor(x => x)
                .Must(task => !task.IsCompleted || task.IsCompleted == false)
                .WithMessage("Cannot create a task that is already completed");
        }
    }
}
