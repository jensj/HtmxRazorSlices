using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public class ToDoDb : IToDoDb
{
    private readonly List<ToDo> _todos = [];

    public Task<IOrderedEnumerable<ToDo>> GetAllToDosAsync(string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_todos.Where(t => t.UserId == userId).OrderBy(t => t.DueDate));
    }

    public Task<ToDo?> GetToDoAsync(string id, string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_todos.Find(x => x.Id == id && x.UserId == userId));
    }

    public Task<ToDo> CreateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        _todos.Add(todo);
        return Task.FromResult(todo);
    }

    public Task<ToDo> UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        _todos.RemoveAll(x => x.Id == todo.Id && x.UserId == todo.UserId);
        _todos.Add(todo);
        return Task.FromResult(todo);
    }

    public Task DeleteToDoAsync(string id, string userId, CancellationToken cancellationToken)
    {
        _todos.RemoveAll(x => x.Id == id && x.UserId == userId);
        return Task.CompletedTask;
    }
}