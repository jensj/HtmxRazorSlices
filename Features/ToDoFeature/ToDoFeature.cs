using FluentValidation;
using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Features.ToDoFeature.Commands;
using HtmxRazorSlices.Features.ToDoFeature.Models;
using HtmxRazorSlices.Features.ToDoFeature.Queries;
using HtmxRazorSlices.Lib;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HtmxRazorSlices.Features.ToDoFeature;

public static class ToDoFeature
{
    private const string RouteBasePath = "/todos";

    public const string TemplatePath = $"/Features/{nameof(ToDoFeature)}/Templates/";

    /// <summary>
    ///     Registers the ToDo feature in the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void RegisterToDoFeature(this WebApplication app)
    {
        // CREATE
        app.MapGet($"{RouteBasePath}/create", () => Results.Extensions.RazorSlice($"{TemplatePath}Create.cshtml", new CreateToDoModel()));
        app.MapPost($"{RouteBasePath}", async (IMediator mediator, [FromForm] CreateToDoModel model, IValidator<CreateToDoModel> validator, HttpContext context) =>
        {
            model.AddValidationResult(await validator.ValidateAsync(model));

            if (model.Errors.Count != 0) return Results.Extensions.RazorSlice($"{TemplatePath}Create.cshtml", model);

            var result = await mediator.Send(new CreateToDoCommand { Description = model.Description, Due = DateOnly.Parse(model.Due) });

            if (result.IsSuccess)
            {
                context.Trigger(TodoEvents.ToDosChanged);
                return await ToDosList(mediator);
            }

            return result.ToErrorResponse();
        });

        // READ
        app.MapGet(RouteBasePath, async (IMediator mediator, HttpContext context, [FromQuery] string? q) =>
        {
            var result = await mediator.Send(new GetToDosQuery(q), CancellationToken.None);

            if (result.IsSuccess)
            {
                // Request was triggered by the search box, return only the rows
                if (context.Request.Headers["HX-Trigger"].Contains("q"))
                    return Results.Extensions.RazorSlice($"{TemplatePath}_rows.cshtml", result.Value);

                return Results.Extensions.RazorSlice($"{TemplatePath}List.cshtml", new ListModel { ToDos = result.Value, Filter = q });
            }

            return result.ToErrorResponse();
        });
        app.MapGet($"{RouteBasePath}/{{id}}", async (IMediator mediator, string id) =>
        {
            var result = await mediator.Send(new GetToDoQuery(id), CancellationToken.None);

            if (result.IsSuccess)
                return Results.Extensions.RazorSlice($"{TemplatePath}Detail.cshtml", new ViewToDo(result.Value));

            return result.ToErrorResponse();
        });

        // UPDATE
        app.MapGet($"{RouteBasePath}/{{id}}/edit", async (IMediator mediator, string id) =>
        {
            var result = await mediator.Send(new GetToDoQuery(id), CancellationToken.None);

            if (result.IsSuccess)
            {
                var toDo = result.Value;
                var model = new EditToDo
                {
                    Id = toDo.Id,
                    Due = toDo.DueDate.ToString("yyyy-MM-dd"),
                    CompletedDate = toDo.CompletedDate?.ToString("yyyy-MM-dd"),
                    Description = toDo.Description
                };
                return Results.Extensions.RazorSlice($"{TemplatePath}Edit.cshtml", model);
            }

            return result.ToErrorResponse();
        });
        app.MapPut($"{RouteBasePath}/{{id}}", async (IMediator mediator, [FromRoute] string id, [FromForm] EditToDo model, IValidator<EditToDo> validator, HttpContext context) =>
        {
            model.Id = id;
            model.AddValidationResult(await validator.ValidateAsync(model));

            if (model.HasErrors) return Results.Extensions.RazorSlice($"{TemplatePath}Edit.cshtml", model);

            var result = await mediator.Send(
                new UpdateToDoCommand
                {
                    Id = model.Id,
                    Description = model.Description,
                    Due = DateOnly.Parse(model.Due),
                    CompletedDate = model.CompletedDate != null ? DateOnly.Parse(model.CompletedDate) : null
                }, CancellationToken.None);

            if (result.IsFailed) return Results.BadRequest(result.Errors.Select(error => error.Message));

            context.Trigger(TodoEvents.ToDosChanged);

            return await ToDosList(mediator);
        });

        // DELETE
        app.MapGet($"{RouteBasePath}/{{id}}/delete", async (IMediator mediator, string id) =>
        {
            var result = await mediator.Send(new GetToDoQuery(id), CancellationToken.None);

            if (result.IsSuccess)
                return Results.Extensions.RazorSlice($"{TemplatePath}Delete.cshtml",
                    new ViewToDo(result.Value));

            return Results.BadRequest(result.Errors.Select(error => error.Message));
        });
        app.MapDelete($"{RouteBasePath}/{{id}}", async (IMediator mediator, string id, HttpContext context) =>
        {
            var result = await mediator.Send(new DeleteToDoCommand(id), CancellationToken.None);

            if (result.IsSuccess)
            {
                context.Trigger(TodoEvents.ToDosChanged);

                return await ToDosList(mediator);
            }

            return Results.BadRequest(result.Errors.Select(error => error.Message));
        });

        // ACTIONS
        app.MapPost($"{RouteBasePath}/{{id}}/complete", async (IMediator mediator, string id, HttpContext context) =>
        {
            var result = await mediator.Send(new ToDoSetStatusCommand { Id = id, Status = ToDo.Complete }, CancellationToken.None);

            if (result.IsSuccess)
            {
                context.Trigger(TodoEvents.ToDosChanged);
                return await ToDosList(mediator);
            }

            return result.ToErrorResponse();
        });
        app.MapPost($"{RouteBasePath}/{{id}}/reopen", async (IMediator mediator, string id, HttpContext context) =>
        {
            var result = await mediator.Send(new ToDoSetStatusCommand { Id = id, Status = ToDo.Reopen }, CancellationToken.None);

            if (result.IsSuccess)
            {
                context.Trigger(TodoEvents.ToDosChanged);
                return await ToDosList(mediator);
            }

            return result.ToErrorResponse();
        });
    }

    // Helper method to return the list of ToDos
    private static async Task<IResult> ToDosList(IMediator mediator)
    {
        var result = await mediator.Send(new GetToDosQuery(), CancellationToken.None);

        if (result.IsSuccess)
            return Results.Extensions.RazorSlice($"{TemplatePath}List.cshtml", new ListModel { ToDos = result.Value });

        return result.ToErrorResponse();
    }
}