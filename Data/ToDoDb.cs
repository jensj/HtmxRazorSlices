using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Data;

public class ToDoDb : IToDoDb
{
    private readonly List<ToDo> _todos = [new ToDo { Description = "Feed the cat", DueDate = DateOnly.FromDateTime(DateTime.Now) }, new ToDo { Description = "Water the plants", DueDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2) }];

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