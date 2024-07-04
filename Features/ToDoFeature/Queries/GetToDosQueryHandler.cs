using FluentResults;
using HtmxRazorSlices.Data;
using HtmxRazorSlices.Features.ToDoFeature.Models;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public class GetToDosQueryHandler(IToDoDb db) : IRequestHandler<GetToDosQuery, Result<IEnumerable<ViewToDo>>>
{
    public async Task<Result<IEnumerable<ViewToDo>>> Handle(GetToDosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var toDos = await db.GetAllToDosAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Filter))
                toDos = toDos
                    .Where(toDo => toDo.Description.Contains(request.Filter, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(toDo => toDo.DueDate);

            var viewToDos = await Task.FromResult(toDos.Select(toDo => new ViewToDo(toDo)));
            return Result.Ok(viewToDos);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}