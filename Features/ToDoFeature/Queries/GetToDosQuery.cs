using FluentResults;
using HtmxRazorSlices.Features.ToDoFeature.Models;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public record GetToDosQuery(string? Filter = "") : IRequest<Result<IEnumerable<ViewToDo>>>;