using MediatR;
using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Events;

public class AccountRegistered : INotification
{
    public Account Account { get; }
    public DateTime RegisteredAt { get; }

    public AccountRegistered(Account account, DateTime registeredAt)
    {
        Account = account ?? throw new ArgumentNullException(nameof(account));
        RegisteredAt = registeredAt;
    }
}