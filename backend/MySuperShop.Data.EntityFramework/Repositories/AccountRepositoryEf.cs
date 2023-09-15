using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Data.EntityFramework.Repositories;

public class AccountRepositoryEf : EfRepository<Account>, IAccountRepository
{
    public AccountRepositoryEf(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
    {
        if (email == null) 
            throw new ArgumentNullException(nameof(email));
        
        return await Entities.SingleAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken)
    {
        if (email == null) 
            throw new ArgumentNullException(nameof(email));
        return await Entities.SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<Account[]?> GetAllAccounts(CancellationToken cancellationToken)
    {
        return await Entities.ToArrayAsync(cancellationToken);
    }
}