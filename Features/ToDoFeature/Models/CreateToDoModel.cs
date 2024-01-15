using System.Collections;
using FluentValidation.Results;

namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class CreateToDoModel : ValidatableModel
{
    public string Description { get; set; } = string.Empty;
    public string Due { get; set; } = string.Empty;

    
}