using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public class ToDoDb : IToDoDb
{
    private readonly List<ToDo> _todos = new() { new ToDo() { Description = "aaa" }, new ToDo() { Description = "bbb" } };

    public Task<IOrderedEnumerable<ToDo>> GetAllToDosAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_todos.OrderBy(t => t.DueDate));
    }

    public Task<ToDo?> GetToDoAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_todos.Find(x => x.Id == id));
    }

    public Task<ToDo> CreateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        _todos.Add(todo);
        return Task.FromResult(todo);
    }

    public Task<ToDo> UpdateToDoAsync(ToDo todo, CancellationToken cancellationToken)
    {
        _todos.RemoveAll(x => x.Id == todo.Id);
        _todos.Add(todo);
        return Task.FromResult(todo);
    }

    public Task DeleteToDoAsync(string id, CancellationToken cancellationToken)
    {
        _todos.RemoveAll(x => x.Id == id);
        return Task.CompletedTask;
    }
}