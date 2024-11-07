namespace TodosApi.ApiContracts;

public sealed record UpdateTodoApiRequest(string Title, bool IsDone);
