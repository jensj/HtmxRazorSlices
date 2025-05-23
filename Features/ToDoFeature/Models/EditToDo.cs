namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class EditToDoModel : ValidatableModel
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Due { get; set; } = string.Empty;
    public string? CompletedDate { get; set; }

    
}