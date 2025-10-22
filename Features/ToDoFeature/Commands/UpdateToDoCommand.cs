using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class UpdateToDoCommand : IRequest<Result<ToDo>>
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly Due { get; set; }
    public DateOnly? CompletedDate { get; set; }
}