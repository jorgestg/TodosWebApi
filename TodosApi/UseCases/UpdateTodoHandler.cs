using FluentValidation;
using MediatR;
using TodosApi.Domain;
using TodosApi.Repositories;
using TodosApi.Validation;

namespace TodosApi.UseCases;

public sealed class UpdateTodoHandler(IValidator<UpdateTodoCommand> validator, ITodoRepository repository)
    : IRequestHandler<UpdateTodoCommand, Result<TodoItem>>
{
    private readonly IValidator<UpdateTodoCommand> _validator = validator;
    private readonly ITodoRepository _repository = repository;

    public async Task<Result<TodoItem>> Handle(UpdateTodoCommand request, CancellationToken ct)
    {
        if (!_validator.IsValid(request, out var validationError))
        {
            return validationError;
        }

        var todoItem = await _repository.Update(request.Id, request.Title, request.IsDone);
        if (todoItem == null)
        {
            return NotFoundError.Instance;
        }

        return todoItem;
    }
}
