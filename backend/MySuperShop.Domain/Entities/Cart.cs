using System.Runtime.CompilerServices;

namespace MySuperShop.Domain.Entities;

public class Cart : IEntity
{
    public Cart(Guid id, Guid accountId)
    {
        Id = id;
        AccountId = accountId;
        Items = new List<CartItem>();
    }
    public void AddItem(Guid productId, double quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (Items == null) throw new InvalidOperationException("Cart is null");
        
        var existedItem = Items!.SingleOrDefault(item => item.ProductId == productId);
        if (existedItem is null)
        {
            Items.Add(new CartItem(Guid.Empty, productId, quantity));
        }
        else
        {
            existedItem.Quantity += quantity;
        }
    }
    
    public Guid Id { get; init; }
    public Guid AccountId { get; set; }
 
    // Обратное навигационное свойство
    public List<CartItem>? Items { get; set; }
}

public record CartItem : IEntity
{
    protected CartItem(){}

    public CartItem(Guid id, Guid productId, double quantity)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
    }

    public Guid Id { get; init; }
    
    // ProductId - это внешний ключ, который ведет на Product.Id
    public Guid ProductId { get; init; }
        
    public double Quantity { get; set; }
    
    // Навигационное свойство
    // В БД превращается в CardId
    public Cart Cart { get; set; } = null!;
}