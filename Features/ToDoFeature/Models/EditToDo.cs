namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class EditToDo : ValidatableModel
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Due { get; set; }
    public string? CompletedDate { get; set; }

    
}