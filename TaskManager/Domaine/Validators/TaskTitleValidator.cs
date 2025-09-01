using FluentValidation;
using TaskManager.Domaine.ValueObjects;

namespace TaskManager.Domaine.Validators
{
    public class TaskTitleValidator : AbstractValidator<TaskTitle>
    {
        public TaskTitleValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("Task title cannot be empty")
                .MaximumLength(200)
                .WithMessage("Task title cannot exceed 200 characters");
        }
    }
}
