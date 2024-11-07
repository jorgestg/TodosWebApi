using Dapper;
using Microsoft.Data.Sqlite;
using TodosApi.Domain;

namespace TodosApi.Repositories;

public sealed class TodoRepository(IConfiguration configuration) : ITodoRepository, IDisposable
{
    private readonly SqliteConnection _connection = new(configuration.GetConnectionString("Default"));

    public Task<TodoItem> Create(string title)
    {
        return _connection.QueryFirstAsync<TodoItem>(
            "INSERT INTO Todos(Title) VALUES (@Title) RETURNING *",
            new { Title = title }
        );
    }

    public async Task<bool> Delete(uint id)
    {
        var rowsAffected = await _connection.ExecuteAsync("DELETE FROM Todos WHERE Id = @Id", new { Id = id });
        return rowsAffected == 1;
    }

    public Task<bool> Exists(string title)
    {
        return _connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Todos WHERE Title = @Title COLLATE NOCASE",
            new { Title = title }
        );
    }

    public Task<TodoItem?> Find(uint id)
    {
        return _connection.QueryFirstOrDefaultAsync<TodoItem?>("SELECT * FROM Todos WHERE Id = @Id", new { Id = id });
    }

    public Task<IEnumerable<TodoItem>> FindAll()
    {
        return _connection.QueryAsync<TodoItem>("SELECT * FROM Todos");
    }

    public Task<TodoItem?> Update(uint id, string title, bool isDone)
    {
        return _connection.QueryFirstOrDefaultAsync<TodoItem>(
            "UPDATE Todos SET Title = @Title, IsDone = @IsDone WHERE Id = @Id RETURNING *",
            new
            {
                Title = title,
                IsDone = isDone,
                Id = id,
            }
        );
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
