using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using TodosApi.ApiContracts;
using TodosApi.ApiEndpoints;
using TodosApi.Domain;
using TodosApi.Shared;
using TodosApi.UseCases;

namespace TodosApi.Tests.ApiEndpoints;

public class TodosEndpointsTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    [Fact]
    public async Task Create_GivenAnOkFromTheHandler_ReturnsCreatedWithTheTodoItem()
    {
        // Arrange
        var request = new CreateTodoApiRequest(TestData.TodoTitle);
        _mediator.Send(Arg.Any<CreateTodoCommand>()).Returns(TestData.TodoItem);

        // Act
        var result = await TodosEndpoints.Create(request, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.Created($"/todos/{TestData.TodoItem.Id}", TestData.TodoItem), result, strict: true);
    }

    [Fact]
    public async Task Create_GivenAValidationErrorFromTheHandler_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateTodoApiRequest(string.Empty);
        _mediator.Send(Arg.Any<CreateTodoCommand>()).Returns(new ValidationError([]));

        // Act
        var result = await TodosEndpoints.Create(request, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.ValidationProblem(new Dictionary<string, string[]>()), result, strict: true);
    }

    [Fact]
    public async Task FindAll_ReturnsTheResultsOfTheHandler()
    {
        // Arrange
        IEnumerable<TodoItem> expectedTodos = [TestData.TodoItem];
        _mediator.Send(FindAllTodosQuery.Instance).Returns(expectedTodos);

        // Act
        var todos = await TodosEndpoints.FindAll(_mediator, CancellationToken.None);

        // Assert
        Assert.Same(expectedTodos, todos);
    }

    [Fact]
    public async Task Find_GivenAnOkFromTheHandler_ReturnsOkWithTheTodoItem()
    {
        // Arrange
        _mediator.Send(new FindTodoQuery(1)).Returns(TestData.TodoItem);

        // Act
        var result = await TodosEndpoints.Find(1, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.Ok(TestData.TodoItem), result, strict: true);
    }

    [Fact]
    public async Task Find_GivenANotFoundErrorFromTheHandler_ReturnsNotFound()
    {
        // Arrange
        _mediator.Send(new FindTodoQuery(1)).Returns(NotFoundError.Instance);

        // Act
        var result = await TodosEndpoints.Find(1, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.NotFound(), result, strict: true);
    }

    [Fact]
    public async Task Update_GivenAnOkFromTheHandler_ReturnsOkWithTheTodoItem()
    {
        // Arrange
        var request = new UpdateTodoApiRequest(TestData.TodoTitle, IsDone: true);
        _mediator.Send(Arg.Any<UpdateTodoCommand>()).Returns(TestData.TodoItem);

        // Act
        var result = await TodosEndpoints.Update(1, request, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.Ok(TestData.TodoItem), result, strict: true);
    }

    [Fact]
    public async Task Update_GivenAValidationErrorFromTheHandler_ReturnsBadRequest()
    {
        // Arrange
        var request = new UpdateTodoApiRequest(string.Empty, IsDone: true);
        _mediator.Send(Arg.Any<UpdateTodoCommand>()).Returns(new ValidationError([]));

        // Act
        var result = await TodosEndpoints.Update(1, request, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.ValidationProblem(new Dictionary<string, string[]>()), result, strict: true);
    }

    [Fact]
    public async Task Update_GivenANotFoundErrorFromTheHandler_ReturnsNotFound()
    {
        // Arrange
        var request = new UpdateTodoApiRequest(TestData.TodoTitle, IsDone: true);
        _mediator.Send(Arg.Any<UpdateTodoCommand>()).Returns(NotFoundError.Instance);

        // Act
        var result = await TodosEndpoints.Update(1, request, _mediator, CancellationToken.None);

        // Assert
        Assert.Equal(Results.NotFound(), result);
    }

    [Fact]
    public async Task Delete_GivenAnOkFromTheHandler_ReturnsNoContent()
    {
        // Arrange
        _mediator.Send(new DeleteTodoCommand(1)).Returns(Unit.Value);

        // Act
        var result = await TodosEndpoints.Delete(1, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.NoContent(), result, strict: true);
    }

    [Fact]
    public async Task Delete_GivenANotFoundErrorFromTheHandler_ReturnsNotFound()
    {
        // Arrange
        _mediator.Send(new DeleteTodoCommand(1)).Returns(NotFoundError.Instance);

        // Act
        var result = await TodosEndpoints.Delete(1, _mediator, CancellationToken.None);

        // Assert
        Assert.Equivalent(Results.NotFound(), result, strict: true);
    }
}
