using NSubstitute;
using TodosApi.Domain;
using TodosApi.Repositories;
using TodosApi.UseCases;

namespace TodosApi.Tests.UseCases;

public class FindAllTodosHandlerTests
{
    private readonly ITodoRepository _repository;
    private readonly FindAllTodosHandler _sut;

    public FindAllTodosHandlerTests()
    {
        _repository = Substitute.For<ITodoRepository>();
        _sut = new FindAllTodosHandler(_repository);
    }

    [Fact]
    public async Task Handle_GivenAQuery_FetchesFromTheRepository()
    {
        // Arrange
        var query = FindAllTodosQuery.Instance;
        IEnumerable<TodoItem> expectedTodos = [TestData.TodoItem];
        _repository.FindAll().Returns(expectedTodos);

        // Act
        var todos = await _sut.Handle(query, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(expectedTodos, todos);
    }
}
