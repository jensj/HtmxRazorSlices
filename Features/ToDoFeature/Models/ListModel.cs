namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class ListModel
{
    public string? Filter { get; init; }
    public IEnumerable<ViewToDo> ToDos { get; init; } = Enumerable.Empty<ViewToDo>();
}