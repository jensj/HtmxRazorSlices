using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class ViewToDo(ToDo toDo)
{
    public string Id { get; set; } = toDo.Id;
    public string Description { get; set; } = toDo.Description;
    public DateOnly Due { get; set; } = toDo.DueDate;
    public DateOnly? CompletedDate { get; set; } = toDo.CompletedDate;
    public bool IsCompleted => CompletedDate.HasValue;
}
