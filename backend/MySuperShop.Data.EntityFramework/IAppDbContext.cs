using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Entities;

namespace MySuperShop.Data.EntityFramework;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartsItems { get; }
    DbSet<ConfirmationCode> ConfirmationCodes { get; }
    
    int SaveChanges();
}