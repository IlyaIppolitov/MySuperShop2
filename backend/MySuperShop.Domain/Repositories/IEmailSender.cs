namespace MySuperShop.Domain.Repositories;


public interface IEmailSender
{
    Task SendEmailAsync(string recepientEmail, string subject, string? body, CancellationToken cancellationToken);
}