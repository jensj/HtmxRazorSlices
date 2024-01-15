using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public record DeleteToDoCommand(string Id) : IRequest<Result>
{
}