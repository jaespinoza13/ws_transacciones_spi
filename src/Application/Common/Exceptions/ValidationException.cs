using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationException : ApplicationException
{
    private ValidationException() : base( "Error en validación de datos revisar los campos enviados" )
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy( e => e.PropertyName, e => e.ErrorMessage )
            .ToDictionary( failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray() );
    }

    public IDictionary<string, string[]> Errors { get; }
}