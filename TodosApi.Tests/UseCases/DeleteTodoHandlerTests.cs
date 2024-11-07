using NSubstitute;
using TodosApi.Repositories;
using TodosApi.Shared;
using TodosApi.UseCases;

namespace TodosApi.Tests.UseCases;

public class DeleteTodoHandlerTests
{
    private readonly ITodoRepository _repository;
    private readonly DeleteTodoHandler _sut;

    public DeleteTodoHandlerTests()
    {
        _repository = Substitute.For<ITodoRepository>();
        _sut = new DeleteTodoHandler(_repository);
    }

    [Fact]
    public async Task Handle_GivenAFoundTodoId_ReturnsASuccessResult()
    {
        // Arrange
        var command = new DeleteTodoCommand(1);
        _repository.Delete(1).Returns(true);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_GivenANotFoundTodoId_ReturnsANotFoundError()
    {
        // Arrange
        var command = new DeleteTodoCommand(1);
        _repository.Delete(1).Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(NotFoundError.Instance, result.Error);
    }
}
