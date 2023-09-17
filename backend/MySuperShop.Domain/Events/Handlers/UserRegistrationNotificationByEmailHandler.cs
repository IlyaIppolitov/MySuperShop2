using MediatR;
using Microsoft.Extensions.Logging;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Domain.Events.Handlers;

public class UserRegistrationNotificationByEmailHandler : INotificationHandler<AccountRegistered>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<UserRegistrationNotificationByEmailHandler> _logger;

    public UserRegistrationNotificationByEmailHandler(
        IEmailSender emailSender,
        ILogger<UserRegistrationNotificationByEmailHandler> _logger)
    {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    }
    
    public async Task Handle(AccountRegistered notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start of email sending about success of Registration");
        await _emailSender.SendEmailAsync(
            notification.Account.Email,
            "Подтверждение рагистрации",
            "Вы успешно зарегистрированы",
            cancellationToken);
    }
}