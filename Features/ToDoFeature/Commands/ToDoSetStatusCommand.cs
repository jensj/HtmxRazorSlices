using FluentResults;
using HtmxRazorSlices.Domain;

using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class ToDoSetStatusCommand : IRequest<Result<ToDo>>
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}