namespace MySuperShop.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository AccountRepository { get; }
    ICartRepository CartRepository { get; }
    IConfirmationCodeRepository ConfirmationCodeRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}