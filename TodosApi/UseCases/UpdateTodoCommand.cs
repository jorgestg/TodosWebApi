using MediatR;
using TodosApi.Domain;

namespace TodosApi.UseCases;

public sealed record UpdateTodoCommand(uint Id, string Title, bool IsDone) : IRequest<Result<TodoItem>>;
