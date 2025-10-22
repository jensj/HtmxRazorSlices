using System.Text.Json;
using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public class FileSystemToDoDb : IToDoDb
{
    private readonly string _storageBasePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public FileSystemToDoDb(string storageBasePath)
    {
        _storageBasePath = storageBasePath ?? throw new ArgumentNullException(nameof(storageBasePath));
        
        if (!Directory.Exists(_storageBasePath))
        {
            Directory.CreateDirectory(_storageBasePath);
        }
    }

    public async Task<IOrderedEnumerable<ToDo>> GetAllToDosAsync(string userId, CancellationToken cancellationToken)
    {
        var todos = await LoadUserToDosAsync(userId, cancellationToken);
        return todos.OrderBy(t => t.DueDate);
    }

    public async Task<ToDo?> GetToDoAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var todos = await LoadUserToDosAsync(userId, cancellationToken);
        return todos.FirstOrDefault(t => t.Id == id);
    }

    public async Task<ToDo> CreateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var todos = await LoadUserToDosAsync(todo.UserId, cancellationToken);
            todos.Add(todo);
            await SaveUserToDosAsync(todo.UserId, todos, cancellationToken);
            return todo;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<ToDo> UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var todos = await LoadUserToDosAsync(todo.UserId, cancellationToken);
            todos.RemoveAll(t => t.Id == todo.Id);
            todos.Add(todo);
            await SaveUserToDosAsync(todo.UserId, todos, cancellationToken);
            return todo;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DeleteToDoAsync(string id, string userId, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var todos = await LoadUserToDosAsync(userId, cancellationToken);
            todos.RemoveAll(t => t.Id == id);
            await SaveUserToDosAsync(userId, todos, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<ToDo>> LoadUserToDosAsync(string userId, CancellationToken cancellationToken)
    {
        var filePath = GetUserFilePath(userId);
        
        if (!File.Exists(filePath))
        {
            return new List<ToDo>();
        }

        var json = await File.ReadAllTextAsync(filePath, cancellationToken);
        return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>();
    }

    private async Task SaveUserToDosAsync(string userId, List<ToDo> todos, CancellationToken cancellationToken)
    {
        var filePath = GetUserFilePath(userId);
        var json = JsonSerializer.Serialize(todos, JsonOptions);
        await File.WriteAllTextAsync(filePath, json, cancellationToken);
    }

    private string GetUserFilePath(string userId)
    {
        var sanitizedUserId = string.Concat(userId.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'));
        return Path.Combine(_storageBasePath, $"todos_{sanitizedUserId}.json");
    }
}
