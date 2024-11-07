using MediatR;
using TodosApi.Domain;

namespace TodosApi.UseCases;

public sealed record FindAllTodosQuery : IRequest<IEnumerable<TodoItem>>
{
    public static readonly FindAllTodosQuery Instance = new();
}
