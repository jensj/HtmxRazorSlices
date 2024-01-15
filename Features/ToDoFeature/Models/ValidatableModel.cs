using FluentValidation.Results;

namespace HtmxRazorSlices.Features.ToDoFeature.Models;

public class ValidatableModel
{
    public List<ValidationFailure> Errors { get; private set; } = new();

    public bool HasErrors => Errors.Count > 0;

    public void AddValidationResult(ValidationResult validationResult)
    {
        Errors = validationResult.Errors;
    }

    public bool HasErrorsFor(string propertyName)
    {
        return Errors.Exists(validationFailure => validationFailure.PropertyName == propertyName);
    }

    public IEnumerable<ValidationFailure> ErrorsFor(string propertyName)
    {
        return Errors.Where(validationFailure => validationFailure.PropertyName == propertyName);
    }
}