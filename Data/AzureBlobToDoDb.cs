using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public class AzureBlobToDoDb : IToDoDb
{
    private readonly BlobContainerClient _containerClient;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public AzureBlobToDoDb(string connectionString, string containerName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrWhiteSpace(containerName))
            throw new ArgumentNullException(nameof(containerName));

        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
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
        var blobName = GetUserBlobName(userId);
        var blobClient = _containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return new List<ToDo>();
        }

        var response = await blobClient.DownloadContentAsync(cancellationToken);
        var json = response.Value.Content.ToString();
        return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>();
    }

    private async Task SaveUserToDosAsync(string userId, List<ToDo> todos, CancellationToken cancellationToken)
    {
        var blobName = GetUserBlobName(userId);
        var blobClient = _containerClient.GetBlobClient(blobName);
        
        var json = JsonSerializer.Serialize(todos, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        using var stream = new MemoryStream(bytes);
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
    }

    private static string GetUserBlobName(string userId)
    {
        var sanitizedUserId = string.Concat(userId.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'));
        return $"todos_{sanitizedUserId}.json";
    }
}
