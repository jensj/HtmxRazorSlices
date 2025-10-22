using HtmxRazorSlices.Data;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class DeleteToDoCommandHandler(IToDoDb db) : IRequestHandler<DeleteToDoCommand, Result>
{
    public async Task<Result> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
    {
        if ( await db.GetToDoAsync(request.Id, request.UserId, cancellationToken) == null) return new ErrorResult("Not found");
        await db.DeleteToDoAsync(request.Id, request.UserId, cancellationToken);
        return new SuccessResult();
    }
}