using FluentResults;
using HtmxRazorSlices.Domain;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public record GetToDoQuery(string Id) : IRequest<Result<ToDo>>;