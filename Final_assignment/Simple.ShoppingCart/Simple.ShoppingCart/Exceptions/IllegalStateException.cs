
namespace Simple.ShoppingCart.Exceptions;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-localized-exception-messages
/// </summary>
[Serializable]
public class IllegalStateException : Exception
{
    public IllegalStateException()
    {
    }

    public IllegalStateException(string message)
        : base(message)
    {
    }

    public IllegalStateException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
