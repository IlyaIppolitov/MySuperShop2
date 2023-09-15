using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Data.EntityFramework.Repositories;

public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
{
    public CartRepositoryEf(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken)
    {
        var cart = await Entities
            .Include(it => it.Items)
            .SingleOrDefaultAsync(it => it.AccountId == id, cancellationToken);
        if (cart is null)
        {
            throw new AccountNotFoundException("Account with given email not found");
        }
        return cart;
    }
}