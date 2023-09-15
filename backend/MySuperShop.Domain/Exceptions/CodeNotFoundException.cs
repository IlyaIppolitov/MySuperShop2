namespace MySuperShop.Domain.Exceptions;

public class CodeNotFoundException: DomainException
{
    public CodeNotFoundException(string message) : base(message)
    {
        
    }
}