using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
    Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken);
    Task<Account[]?> GetAllAccounts(CancellationToken cancellationToken);
}