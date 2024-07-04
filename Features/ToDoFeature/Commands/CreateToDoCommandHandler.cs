using FluentResults;
using HtmxRazorSlices.Data;
using HtmxRazorSlices.Domain;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class CreateToDoCommandHandler(IToDoDb db) : IRequestHandler<CreateToDoCommand, Result<ToDo>>
{
    public async Task<Result<ToDo>> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = new ToDo { Description = request.Description, DueDate = request.Due };

        await db.CreateToDoAsync(toDo, cancellationToken);

        return Result.Ok(toDo);
    }
}