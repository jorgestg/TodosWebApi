using NSubstitute;
using TodosApi.Domain;
using TodosApi.Repositories;
using TodosApi.Shared;
using TodosApi.UseCases;

namespace TodosApi.Tests.UseCases;

public sealed class FindTodoHandlerTests
{
    private readonly ITodoRepository _repository;
    private readonly FindTodoHandler _sut;

    public FindTodoHandlerTests()
    {
        _repository = Substitute.For<ITodoRepository>();
        _sut = new FindTodoHandler(_repository);
    }

    [Fact]
    public async Task Handle_GivenAFoundId_ReturnsTheFoundTodoItem()
    {
        // Arrange
        var query = new FindTodoQuery(1);
        var todoItem = TestData.TodoItem;
        _repository.Find(query.Id).Returns(todoItem);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(todoItem, result.OkValue);
    }

    [Fact]
    public async Task Handle_GivenANotFoundId_ReturnsANotFoundError()
    {
        // Arrange
        var query = new FindTodoQuery(1);
        _repository.Find(query.Id).Returns((TodoItem?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(NotFoundError.Instance, result.Error);
    }
}
