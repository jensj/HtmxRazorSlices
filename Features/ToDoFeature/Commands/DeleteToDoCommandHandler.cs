using FluentResults;
using HtmxRazorSlices.Data;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class DeleteToDoCommandHandler(IToDoDb db) : IRequestHandler<DeleteToDoCommand, Result>
{
    public async Task<Result> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
    {
        if ( await db.GetToDoAsync(request.Id, cancellationToken) == null) return Result.Fail("Not found");
        await db.DeleteToDoAsync(request.Id, cancellationToken);
        return Result.Ok();
    }
}