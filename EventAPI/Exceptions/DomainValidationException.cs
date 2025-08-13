namespace EventAPI.Exceptions
{
    public class DomainValidationException:Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public DomainValidationException(IDictionary<string, string[]> errors, string? message = null)
            : base(message ?? "Validation failed")
        {
            Errors = errors;
        }
    }
}
