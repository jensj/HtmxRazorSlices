using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class UpdateToDoCommand : IRequest<Result<ToDo>>
{
    public string Id { get; set; }
    public string Description { get; set; }
    public DateOnly Due { get; set; }
    public DateOnly? CompletedDate { get; set; }
}