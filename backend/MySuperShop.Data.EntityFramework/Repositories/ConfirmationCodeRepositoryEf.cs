using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Data.EntityFramework.Repositories;

public class ConfirmationCodeRepositoryEf : EfRepository<ConfirmationCode>, IConfirmationCodeRepository
{
    public ConfirmationCodeRepositoryEf(AppDbContext dbContext) : base(dbContext)
    {
    }
}