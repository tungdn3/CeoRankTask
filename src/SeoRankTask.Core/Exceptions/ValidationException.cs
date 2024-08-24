using FluentValidation.Results;

namespace CeoRankTask.Core.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationFailure> Errors { get; set; }

    public ValidationException(List<ValidationFailure> errors)
    {
        Errors = errors;
    }

    public ValidationException(string? message, List<ValidationFailure> errors) : base(message)
    {
        Errors = errors;
    }

    public ValidationException(string? message, Exception? innerException, List<ValidationFailure> errors) : base(message, innerException)
    {
        Errors = errors;
    }
}
