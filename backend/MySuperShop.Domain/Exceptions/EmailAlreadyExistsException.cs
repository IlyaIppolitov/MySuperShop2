using System.Runtime.Serialization;

namespace MySuperShop.Domain.Exceptions;

public class EmailAlreadyExistsException : DomainException
{

    public string Value { get; }
    public EmailAlreadyExistsException(string value)
    {
        Value = value;
    }

    protected EmailAlreadyExistsException(SerializationInfo info, StreamingContext context, string value) : base(info, context)
    {
        Value = value;
    }

    public EmailAlreadyExistsException(string? message, Exception? innerException, string value) : base(message, innerException)
    {
        Value = value;
    }
    public EmailAlreadyExistsException(string message, string val) : base(message)
    {
        Value = val;
    }
}