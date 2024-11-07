using MediatR;

namespace TodosApi.UseCases;

public sealed record DeleteTodoCommand(uint Id) : IRequest<Result<Unit>>;
