using FluentValidation;

namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class EditToDoValidator : AbstractValidator<EditToDoModel>
{
    public EditToDoValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Description).MinimumLength(5);
        RuleFor(x => x.Due).NotEmpty();
        RuleFor(x => x.Due).Must(x => DateOnly.TryParse(x, out _)).WithMessage("Due date must be a valid date.");
    }
}