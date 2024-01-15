﻿using HtmxRazorSlices.Domain;
using HtmxRazorSlices.Lib;
using MediatR;

namespace HtmxRazorSlices.Features.ToDoFeature.Commands;

public class CreateToDoCommand : IRequest<Result<ToDo>>
{
    public string Description { get; set; }

    public DateOnly Due { get; set; }
}