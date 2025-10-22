using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Features.ToDoFeature.Models;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public record GetAllToDosQuery(string UserId, string? Filter = "") : IRequest<IEnumerable<ViewToDo>>;