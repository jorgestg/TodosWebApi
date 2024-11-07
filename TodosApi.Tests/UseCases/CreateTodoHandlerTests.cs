using NSubstitute;
using TodosApi.Repositories;
using TodosApi.Shared;
using TodosApi.UseCases;
using TodosApi.Validation;

namespace TodosApi.Tests.UseCases;

public class CreateTodoHandlerTests
{
    private readonly CreateTodoValidator _validator;
    private readonly ITodoRepository _repository;
    private readonly CreateTodoHandler _sut;

    public CreateTodoHandlerTests()
    {
        _validator = new CreateTodoValidator();
        _repository = Substitute.For<ITodoRepository>();
        _sut = new CreateTodoHandler(_validator, _repository);
    }

    [Fact]
    public async Task Handle_GivenAValidCommand_StoresInTheRepository()
    {
        // Arrange
        var command = new CreateTodoCommand(TestData.TodoTitle);
        var createdTodoItem = TestData.TodoItem;

        _repository.Create(TestData.TodoTitle).Returns(createdTodoItem);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(1);
        Assert.Same(createdTodoItem, result.OkValue);
    }

    public static readonly TheoryData<string> InvalidTitles = TestData.InvalidTitles();

    [Theory]
    [MemberData(nameof(InvalidTitles))]
    public async Task Handle_GivenAnInvalidCommand_ReturnsAValidationError(string title)
    {
        // Arrange
        var command = new CreateTodoCommand(title);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _repository.Received(0);
        Assert.IsType<ValidationError>(result.Error);
    }
}
