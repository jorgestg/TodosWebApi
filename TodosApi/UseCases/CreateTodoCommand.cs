using MediatR;
using TodosApi.Domain;

namespace TodosApi.UseCases;

public sealed record CreateTodoCommand(string Title) : IRequest<Result<TodoItem>>;
