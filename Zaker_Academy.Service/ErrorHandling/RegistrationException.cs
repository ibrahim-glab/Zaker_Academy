
// add it to namespace 
namespace Zaker_Academy.Service.ErrorHandling{
internal class RegistrationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public RegistrationException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}
}