using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class ToggleToDoCommandHandler(IToDoDb db) : IRequestHandler<ToggleToDoCommand, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(ToggleToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = await db.GetToDoAsync(request.Id, cancellationToken);
        if (toDo == null) return new ErrorResult<ToDo>("Not found");

        toDo.CompletedDate = toDo.CompletedDate.HasValue ? null : DateOnly.FromDateTime(DateTime.Today);

        await db.UpdateToDoAsync(toDo, cancellationToken);
        return new SuccessResult<ToDo>(toDo);
    }
}