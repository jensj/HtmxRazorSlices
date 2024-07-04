using FluentResults;
using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public class GetToDoQueryHandler(IToDoDb db) : IRequestHandler<GetToDoQuery, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(GetToDoQuery request, CancellationToken cancellationToken)
    {
        var toDo = await db.GetToDoAsync(request.Id, cancellationToken);

        return toDo == null ? Result.Fail("Not found") : Result.Ok(toDo);
    }
}