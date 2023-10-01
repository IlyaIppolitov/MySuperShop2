using Microsoft.VisualBasic.CompilerServices;
using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Test;

public class CartTests
{
    [Fact]
    public void Item_is_added_to_cart()
    {
        // Arrange
        var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        var quantity = 1d;
        
        // Act
        cart.AddItem(productId, quantity);
        
        // Assert
        // факт добавления товара
        Assert.Single(cart.Items!);
        
        // Проверка Id товара заданному
        var cartItem = cart.Items!.FirstOrDefault(it => it.ProductId == productId);
        Assert.NotNull(cartItem);
        
        // Проврерка количества добавленного товара
        Assert.Equal(quantity, cartItem.Quantity);
    }

    [Fact]
    public void Adding_existing_item_to_cart_increases_its_quantity()
    {
        // Arrange
        var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        
        // Act
        cart.AddItem(productId, 1d);
        cart.AddItem(productId, 1d);
        
        // Assert
        Assert.Single(cart.Items!);
        Assert.Equal(2, cart.Items!.First().Quantity);
    }
    
    [Fact]
    public void Five_items_added_to_cart()
    {
        // Arrange
        // Создание пяти товаров
        var cartItems = new List<CartItem>();
        for (var i = 1; i < 6; ++i)
        {
            cartItems.Add(new CartItem(Guid.NewGuid(), Guid.NewGuid(), i));
        }
        
        // создание новой корзины
        var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());

        // Act
        // заполнение корзины товарами
        foreach (var it in cartItems)
        {
            cart.AddItem(it.ProductId, it.Quantity);
        }
        
        // Assert
        foreach (var it in cartItems)
        {
            // Проверка Id товара заданному
            var currentItem = cart.Items!.FirstOrDefault(item => item.ProductId == it.ProductId);
            Assert.NotNull(currentItem);
        
            // Проврерка количества добавленного товара
            Assert.Equal(it.Quantity, currentItem.Quantity);
        }
        
        Assert.Equal(cartItems.Count, cart.Items!.Count);
    }
}