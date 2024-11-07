using MediatR;
using TodosApi.Domain;

namespace TodosApi.UseCases;

public sealed record FindTodoQuery(uint Id) : IRequest<Result<TodoItem>>;
