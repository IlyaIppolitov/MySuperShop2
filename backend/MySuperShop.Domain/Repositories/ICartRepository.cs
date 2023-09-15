using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetCartByAccountId (Guid accountId, CancellationToken cancellationToken);
}