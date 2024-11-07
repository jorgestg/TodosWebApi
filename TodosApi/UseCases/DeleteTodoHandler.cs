using MediatR;
using TodosApi.Repositories;

namespace TodosApi.UseCases;

public sealed class DeleteTodoHandler(ITodoRepository repository) : IRequestHandler<DeleteTodoCommand, Result<Unit>>
{
    private readonly ITodoRepository _repository = repository;

    public async Task<Result<Unit>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var found = await _repository.Delete(request.Id);
        if (!found)
        {
            return NotFoundError.Instance;
        }

        return Unit.Value;
    }
}
