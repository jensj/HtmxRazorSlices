using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public interface IToDoDb
{
    Task<IOrderedEnumerable<ToDo>> GetAllToDosAsync(CancellationToken cancellationToken);
    Task<ToDo?> GetToDoAsync(string id, CancellationToken cancellationToken);
    Task<ToDo> CreateToDoAsync(ToDo todo, CancellationToken cancellationToken);
    Task<ToDo> UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken);
    Task DeleteToDoAsync(string id, CancellationToken cancellationToken);
}