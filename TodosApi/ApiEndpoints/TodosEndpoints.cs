using MediatR;
using TodosApi.ApiContracts;
using TodosApi.Domain;
using TodosApi.UseCases;

namespace TodosApi.ApiEndpoints;

public static class TodosEndpoints
{
    public static void MapTodos(this WebApplication app)
    {
        app.MapPost("/todos", Create);
        app.MapGet("/todos", FindAll);
        app.MapGet("/todos/{id:min(1)}", Find);
        app.MapPut("/todos/{id:min(1)}", Update);
        app.MapDelete("/todos/{id:min(1)}", Delete);
    }

    public static async Task<IResult> Create(CreateTodoApiRequest request, IMediator mediator, CancellationToken ct)
    {
        var command = new CreateTodoCommand(request.Title);
        var result = await mediator.Send(command, ct);
        return result.IsSuccess
            ? Results.Created($"/todos/{result.OkValue.Id}", result.OkValue)
            : result.Error.ToHttpResult();
    }

    public static async Task<IEnumerable<TodoItem>> FindAll(IMediator mediator, CancellationToken ct)
    {
        return await mediator.Send(FindAllTodosQuery.Instance, ct);
    }

    public static async Task<IResult> Find(uint id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new FindTodoQuery(id), ct);
        return result.ToOkOrErrorHttpResult();
    }

    public static async Task<IResult> Update(
        uint id,
        UpdateTodoApiRequest request,
        IMediator mediator,
        CancellationToken ct
    )
    {
        var command = new UpdateTodoCommand(id, request.Title, request.IsDone);
        var result = await mediator.Send(command, ct);
        return result.ToOkOrErrorHttpResult();
    }

    public static async Task<IResult> Delete(uint id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteTodoCommand(id), ct);
        return result.IsSuccess ? Results.NoContent() : result.Error.ToHttpResult();
    }
}
