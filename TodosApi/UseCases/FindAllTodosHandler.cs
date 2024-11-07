using MediatR;
using TodosApi.Domain;
using TodosApi.Repositories;

namespace TodosApi.UseCases;

public sealed class FindAllTodosHandler(ITodoRepository repository)
    : IRequestHandler<FindAllTodosQuery, IEnumerable<TodoItem>>
{
    private readonly ITodoRepository _repository = repository;

    public async Task<IEnumerable<TodoItem>> Handle(FindAllTodosQuery request, CancellationToken cancellationToken)
    {
        return await _repository.FindAll();
    }
}
