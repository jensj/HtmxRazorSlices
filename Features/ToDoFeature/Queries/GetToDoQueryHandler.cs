using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Queries;

public class GetToDoQueryHandler(IToDoDb db) : IRequestHandler<GetToDoQuery, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(GetToDoQuery request, CancellationToken cancellationToken)
    {
        var toDo = await db.GetToDoAsync(request.Id, request.UserId, cancellationToken);

        return toDo == null ? new ErrorResult<ToDo>("Not found") : new SuccessResult<ToDo>(toDo);
    }
}