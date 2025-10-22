using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class CreateToDoCommandHandler(IToDoDb db) : IRequestHandler<CreateToDoCommand, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = new ToDo { UserId = request.UserId, Description = request.Description, DueDate = request.Due };

        await db.CreateToDoAsync(toDo, cancellationToken);

        return new SuccessResult<ToDo>(toDo);
    }
}