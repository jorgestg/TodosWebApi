namespace TodosApi.Domain;

public sealed class TodoItem
{
    public uint Id { get; init; }
    public required string Title { get; set; }
    public bool IsDone { get; set; }
}
