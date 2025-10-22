using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class UpdateToDoCommandHandler(IToDoDb db) : IRequestHandler<UpdateToDoCommand, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = await db.GetToDoAsync(request.Id, request.UserId, cancellationToken);
        if (toDo == null) return new ErrorResult<ToDo>("Not found");

        toDo.Description = request.Description;
        toDo.DueDate = request.Due;

        await db.UpdateToDoAsync(toDo, cancellationToken);
        return new SuccessResult<ToDo>(toDo);
    }
}