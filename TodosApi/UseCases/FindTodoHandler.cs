using MediatR;
using TodosApi.Domain;
using TodosApi.Repositories;

namespace TodosApi.UseCases;

public sealed class FindTodoHandler(ITodoRepository repository) : IRequestHandler<FindTodoQuery, Result<TodoItem>>
{
    private readonly ITodoRepository _repository = repository;

    public async Task<Result<TodoItem>> Handle(FindTodoQuery request, CancellationToken ct)
    {
        var todoItem = await _repository.Find(request.Id);
        if (todoItem == null)
        {
            return NotFoundError.Instance;
        }

        return todoItem;
    }
}
