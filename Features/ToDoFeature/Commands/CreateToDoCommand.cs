using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class CreateToDoCommand : IRequest<Result<ToDo>>
{
    public string UserId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateOnly Due { get; set; }
}