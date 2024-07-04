using FluentResults;
using HtmxRazorSlices.Domain;

using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class ToggleToDoCommand : IRequest<Result<ToDo>>
{
    public string Id { get; set; } = string.Empty;
}