using HtmxRazorSlices.Data;
using HtmxRazorSlices.Features.ToDoFeature.Models;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public class GetAllToDosQueryHandler(IToDoDb db) : IRequestHandler<GetAllToDosQuery, IEnumerable<ViewToDo>>
{
    public async Task<IEnumerable<ViewToDo>> Handle(GetAllToDosQuery request, CancellationToken cancellationToken)
    {
        var toDos = await db.GetAllToDosAsync(request.UserId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Filter))
            toDos = toDos
                    .Where(toDo => toDo.Description.Contains(request.Filter, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(toDo => toDo.Id);

        return await Task.FromResult(toDos.Select(toDo => new ViewToDo(toDo)));
    }
}