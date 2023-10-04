using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Events;
using MySuperShop.Domain.Events.Handlers;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Domain.Test;

public class AccountTests
{
    [Fact]
    public async void Account_registered_event_notifies_user_by_email()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), "John", "John@john.com", "qwerty", new[] { Role.Customer });
        
        var loggerMock = new Mock<ILogger<UserRegistrationNotificationByEmailHandler>>();
        
        var emailSenderMock = new Mock<IEmailSender>();
        
        
        // var emailSender = new FakeEmailSender();
        var logger = new Logger<UserRegistrationNotificationByEmailHandler>(new LoggerFactory());
        
        var handler = new UserRegistrationNotificationByEmailHandler(emailSenderMock.Object, loggerMock.Object);
        var @event = new AccountRegistered(account, DateTime.Now);
        
        // Act
        await handler.Handle(@event, default);

        // Assert
        // 1. Активация обработчика (триггер)
        handler.Should().BeAssignableTo<INotificationHandler<AccountRegistered>>();

        // 2. Факт вызова метода отправки сообщения
        emailSenderMock
            .Verify(it => 
                it.SendEmailAsync(account.Email, It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);

        
        
        // emailSender.counter.Should().Be(1);
    }
    
}

// class FakeEmailSender : IEmailSender
// {
//     public int counter
//     {
//         get;
//         set;
//     }
//     public Task SendEmailAsync(string recepientEmail, string subject, string? body, CancellationToken cancellationToken)
//     {
//         counter++;
//         return Task.CompletedTask;
//     }
// }