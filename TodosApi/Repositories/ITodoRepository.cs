using TodosApi.Domain;

namespace TodosApi.Repositories;

public interface ITodoRepository
{
    Task<TodoItem> Create(string title);
    Task<bool> Exists(string title);
    Task<IEnumerable<TodoItem>> FindAll();
    Task<TodoItem?> Find(uint id);
    Task<TodoItem?> Update(uint id, string title, bool isDone);
    Task<bool> Delete(uint id);
}
