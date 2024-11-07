using FluentValidation;
using MediatR;
using TodosApi.Domain;
using TodosApi.Repositories;
using TodosApi.Validation;

namespace TodosApi.UseCases;

public sealed class CreateTodoHandler(IValidator<CreateTodoCommand> validator, ITodoRepository repository)
    : IRequestHandler<CreateTodoCommand, Result<TodoItem>>
{
    private readonly IValidator<CreateTodoCommand> _validator = validator;
    private readonly ITodoRepository _repository = repository;

    public async Task<Result<TodoItem>> Handle(CreateTodoCommand request, CancellationToken ct)
    {
        if (!_validator.IsValid(request, out var validationError))
        {
            return validationError;
        }

        return await _repository.Create(request.Title);
    }
}
