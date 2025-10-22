using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public interface IToDoDb
{
    Task<IOrderedEnumerable<ToDo>> GetAllToDosAsync(string userId, CancellationToken cancellationToken);
    Task<ToDo?> GetToDoAsync(string id, string userId, CancellationToken cancellationToken);
    Task<ToDo> CreateToDoAsync(ToDo todo, CancellationToken cancellationToken);
    Task<ToDo> UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken);
    Task DeleteToDoAsync(string id, string userId, CancellationToken cancellationToken);
}