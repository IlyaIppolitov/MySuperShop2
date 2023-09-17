using MediatR;
using Microsoft.Extensions.Logging;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Domain.Events.Handlers;

public class LoginConfirmationCodeSentByEmailHandler : INotificationHandler<LoginConfirmationCodeSent>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<LoginConfirmationCodeSentByEmailHandler> _logger;

    public LoginConfirmationCodeSentByEmailHandler(
        IEmailSender emailSender,
        ILogger<LoginConfirmationCodeSentByEmailHandler> logger)
    {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task Handle(LoginConfirmationCodeSent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Confirmation code sent to user mail to validate 2ns step of authentication: {CodeCode}", notification.Code.Code);
        await _emailSender.SendEmailAsync(
            notification.Account.Email!,
            "Код подтверждения",
            notification.Code.Code,
            cancellationToken);
    }
}