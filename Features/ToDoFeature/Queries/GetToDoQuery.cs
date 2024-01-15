using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public record GetToDoQuery(string Id) : IRequest<Result<ToDo>>;