using MediatR;
using MySuperShop.Domain.Entities;

namespace MySuperShop.Domain.Events;

public class LoginConfirmationCodeSent : INotification
{
    public Account Account { get; }
    public ConfirmationCode Code { get; }

    public LoginConfirmationCodeSent(Account account, ConfirmationCode code)
    {
        Account = account ?? throw new ArgumentNullException(nameof(account));
        Code = code ?? throw new ArgumentNullException(nameof(code));
    }
}