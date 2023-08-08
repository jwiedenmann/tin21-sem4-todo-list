using System.Globalization;

namespace Pyco.Todo.Core.Exception;

public class UnauthorizedException : System.Exception
{
    public UnauthorizedException() : base() { }

    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}