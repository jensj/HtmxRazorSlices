using FluentResults;
using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;

using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class ToggleToDoCommandHandler(IToDoDb db) : IRequestHandler<ToDoSetStatusCommand, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(ToDoSetStatusCommand request, CancellationToken cancellationToken)
    {
        var toDo = await db.GetToDoAsync(request.Id, cancellationToken);
        if (toDo == null) return Result.Fail("Not found");

        if(request.Status == ToDo.Complete)
            toDo.CompletedDate = DateOnly.FromDateTime(DateTime.Today);
        else
            toDo.CompletedDate = null;

        //toDo.CompletedDate = toDo.CompletedDate.HasValue ? null : DateOnly.FromDateTime(DateTime.Today);

        await db.UpdateToDoAsync(toDo, cancellationToken);
        return Result.Ok(toDo);
    }
}