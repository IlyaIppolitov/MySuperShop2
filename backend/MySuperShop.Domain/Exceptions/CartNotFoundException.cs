namespace MySuperShop.Domain.Exceptions;

public class CartNotFoundException : DomainException
{
    public CartNotFoundException(string message) : base(message)
    {
        
    }
}