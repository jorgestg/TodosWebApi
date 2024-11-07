using FluentValidation;
using TodosApi.UseCases;

namespace TodosApi.Validation;

public sealed class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(TodoItemValidationConstants.TitleMinLength)
            .MaximumLength(TodoItemValidationConstants.TitleMaxLength);
    }
}
