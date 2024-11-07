using FluentValidation;
using TodosApi.UseCases;

namespace TodosApi.Validation;

public sealed class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(TodoItemValidationConstants.TitleMinLength)
            .MaximumLength(TodoItemValidationConstants.TitleMaxLength);
    }
}
