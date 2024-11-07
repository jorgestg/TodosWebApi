using NSubstitute;
using TodosApi.Domain;
using TodosApi.Repositories;
using TodosApi.Shared;
using TodosApi.UseCases;
using TodosApi.Validation;

namespace TodosApi.Tests.UseCases;

public class UpdateTodoHandlerTests
{
    private readonly UpdateTodoValidator _validator;
    private readonly ITodoRepository _repository;
    private readonly UpdateTodoHandler _sut;

    public UpdateTodoHandlerTests()
    {
        _validator = new UpdateTodoValidator();
        _repository = Substitute.For<ITodoRepository>();
        _sut = new UpdateTodoHandler(_validator, _repository);
    }

    [Fact]
    public async Task Handle_GivenAValidCommand_UpdatesInTheRepository()
    {
        // Arrange
        const string newTitle = "New title";
        var command = new UpdateTodoCommand(1, newTitle, IsDone: false);
        var updatedTodoItem = new TodoItem
        {
            Id = command.Id,
            Title = command.Title,
            IsDone = command.IsDone,
        };

        _repository.Update(command.Id, command.Title, command.IsDone).Returns(updatedTodoItem);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(updatedTodoItem, result.OkValue);
    }

    public static readonly TheoryData<string> InvalidTitles = TestData.InvalidTitles();

    [Theory]
    [MemberData(nameof(InvalidTitles))]
    public async Task Handle_GivenAnInvalidCommand_ReturnsAValidationError(string title)
    {
        // Arrange
        var command = new UpdateTodoCommand(1, title, false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(0);
        Assert.IsType<ValidationError>(result.Error);
    }

    [Fact]
    public async Task Handle_GivenAnIdThatIsNotFound_ReturnsANotFoundError()
    {
        // Arrange
        const string newTitle = "New title";
        var command = new UpdateTodoCommand(1, newTitle, IsDone: false);
        _repository.Update(command.Id, command.Title, command.IsDone).Returns((TodoItem?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(NotFoundError.Instance, result.Error);
    }
}
