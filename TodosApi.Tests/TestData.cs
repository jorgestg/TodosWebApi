using TodosApi.Domain;

namespace TodosApi.Tests;

public static class TestData
{
    public const string TodoTitle = "Assess my .NET skills";

    public static readonly TodoItem TodoItem =
        new()
        {
            Id = 1,
            Title = TodoTitle,
            IsDone = false,
        };

    public static TheoryData<string> InvalidTitles()
    {
        return new TheoryData<string> { "a", "aa", new(c: 'a', count: 256) };
    }
}
