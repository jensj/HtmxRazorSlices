namespace HtmxRazorSlices.Domain;

public class ToDo
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Description { get; set; }
    public DateOnly DueDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
    public DateOnly? CompletedDate { get; set; }
}