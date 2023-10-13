using System.Security.AccessControl;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Events;
using MySuperShop.Domain.Events.Handlers;
using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Repositories;
using MySuperShop.Domain.Services;

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
    }

    [Fact]
    public async void New_account_registered()
    {
        // Arrange
        var testAccounts = new List<Account>();
        var testCart = new List<Cart>();
        
        var accountRepMock = new Mock<IAccountRepository>();
        var confCodeRepMock = new Mock<IConfirmationCodeRepository>();

        accountRepMock
            .Setup(x => x.FindAccountByEmail(It.IsAny<string>(), default))
            .Returns((string email, CancellationToken ct) =>
            {
                return Task.FromResult(testAccounts.FirstOrDefault(u => u.Email == email));
            });

        accountRepMock
            .Setup(x => x.Add(It.IsAny<Account>(), default))
            .Callback((Account acc, CancellationToken ct) => testAccounts.Add(acc));
        
        var cartRepMock = new Mock<ICartRepository>();
        cartRepMock
            .Setup(x => x.Add(It.IsAny<Cart>(), default))
            .Callback((Cart cart, CancellationToken ct) => testCart.Add(cart));
        
        
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
        uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);
        
        var hasher = new Mock<IApplicationPasswordHasher>();
        hasher.Setup(x => x.HashPassword((It.IsAny<string>())))
            .Returns((string pass) => pass + "_hashed");

        var userGenerator = new Faker<TestUser>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Roles, () => new [] { Role.Customer});
        var user = userGenerator.Generate();
        
        var loggerMock = new Mock<ILogger<AccountService>>();
        var mediatorMock = new Mock<IMediator>();

        var service = new AccountService(hasher.Object, uowMock.Object, loggerMock.Object, mediatorMock.Object);
        
        // Act
        Func<Task> act = () => service.Register(user.Name, user.Email, user.Password, user.Roles, default);

        // Assert
        // Выполняется без отправки exception
        await act.Should().NotThrowAsync();
        // Создан один аккаунт пользователя и одна корзина account
        testAccounts.Should().HaveCount(1);
        testCart.Should().HaveCount(1);
        // Проверка корректности заполнения полей данных аккаунта
        var firstUser = testAccounts.ElementAt(0);
        firstUser.Name.Should().Be(user.Name);
        firstUser.Email.Should().Be(user.Email);
        firstUser.HashedPassword.Should().Be(user.Password + "_hashed");
        firstUser.Roles.Should().Equal(user.Roles);
        firstUser.Id.Should().NotBeEmpty();
        
        // Проверка вызова обработчика
        mediatorMock.Verify(it => it.Publish(It.IsAny<AccountRegistered>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void Second_account_with_same_email_registration_throws_exception()
    {
        // Arrange
        var testAccounts = new List<Account>();
        var testCart = new List<Cart>();
        
        var accountRepMock = new Mock<IAccountRepository>();
        var confCodeRepMock = new Mock<IConfirmationCodeRepository>();

        accountRepMock
            .Setup(x => x.FindAccountByEmail(It.IsAny<string>(), default))
            .Returns((string email, CancellationToken ct) =>
            {
                return Task.FromResult(testAccounts.FirstOrDefault(u => u.Email == email));
            });

        accountRepMock
            .Setup(x => x.Add(It.IsAny<Account>(), default))
            .Callback((Account acc, CancellationToken ct) => testAccounts.Add(acc));
        
        var cartRepMock = new Mock<ICartRepository>();
        cartRepMock
            .Setup(x => x.Add(It.IsAny<Cart>(), default))
            .Callback((Cart cart, CancellationToken ct) => testCart.Add(cart));
        
        
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
        uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);
        
        var hasher = new Mock<IApplicationPasswordHasher>();
        hasher.Setup(x => x.HashPassword((It.IsAny<string>())))
            .Returns((string pass) => pass + "_hashed");

        var userGenerator = new Faker<TestUser>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Roles, () => new [] { Role.Customer});
        var user = userGenerator.Generate();
        
        var loggerMock = new Mock<ILogger<AccountService>>();
        var mediatorMock = new Mock<IMediator>();

        var service = new AccountService(hasher.Object, uowMock.Object, loggerMock.Object, mediatorMock.Object);
        
        // Act
        Func<Task> act1 = () => service.Register(user.Name, user.Email, user.Password, user.Roles, default);

        // Assert
        // Выполняется без отправки exception
        await act1.Should().NotThrowAsync();
        await act1.Should().ThrowAsync<EmailAlreadyExistsException>();
    }

    [Fact]
    public async void Account_registration_throws_argument_null_exception()
    {
        // Arrange
        var testAccounts = new List<Account>();
        var testCart = new List<Cart>();
        
        var accountRepMock = new Mock<IAccountRepository>();
        var confCodeRepMock = new Mock<IConfirmationCodeRepository>();

        accountRepMock
            .Setup(x => x.FindAccountByEmail(It.IsAny<string>(), default))
            .Returns((string email, CancellationToken ct) =>
            {
                return Task.FromResult(testAccounts.FirstOrDefault(u => u.Email == email));
            });

        accountRepMock
            .Setup(x => x.Add(It.IsAny<Account>(), default))
            .Callback((Account acc, CancellationToken ct) => testAccounts.Add(acc));
        
        var cartRepMock = new Mock<ICartRepository>();
        cartRepMock
            .Setup(x => x.Add(It.IsAny<Cart>(), default))
            .Callback((Cart cart, CancellationToken ct) => testCart.Add(cart));
        
        
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
        uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);
        
        var hasher = new Mock<IApplicationPasswordHasher>();
        hasher.Setup(x => x.HashPassword((It.IsAny<string>())))
            .Returns((string pass) => pass + "_hashed");

        var userGenerator = new Faker<TestUser>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Roles, () => new [] { Role.Customer});
        var user = userGenerator.Generate();
        
        var loggerMock = new Mock<ILogger<AccountService>>();
        var mediatorMock = new Mock<IMediator>();

        var service = new AccountService(hasher.Object, uowMock.Object, loggerMock.Object, mediatorMock.Object);
        
        // Act
        Func<Task> act1 = () => service.Register(user.Name, null, user.Password, user.Roles, default);
        Func<Task> act2 = () => service.Register(user.Name, user.Email, null, user.Roles, default);

        // Assert
        // Выполняется без отправки exception
        await act1.Should().ThrowAsync<ArgumentNullException>();
        await act2.Should().ThrowAsync<ArgumentNullException>();

    }
    
    

    [Fact]
    public async void account_service_throws_null_argument_exception()
    {
        // Arrange
        var testAccounts = new List<Account>();
        var testCart = new List<Cart>();
        
        var accountRepMock = new Mock<IAccountRepository>();
        var confCodeRepMock = new Mock<IConfirmationCodeRepository>();

        accountRepMock
            .Setup(x => x.FindAccountByEmail(It.IsAny<string>(), default))
            .Returns((string email, CancellationToken ct) =>
            {
                return Task.FromResult(testAccounts.FirstOrDefault(u => u.Email == email));
            });

        accountRepMock
            .Setup(x => x.Add(It.IsAny<Account>(), default))
            .Callback((Account acc, CancellationToken ct) => testAccounts.Add(acc));
        
        var cartRepMock = new Mock<ICartRepository>();
        cartRepMock
            .Setup(x => x.Add(It.IsAny<Cart>(), default))
            .Callback((Cart cart, CancellationToken ct) => testCart.Add(cart));
        
        
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
        uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);
        
        var hasher = new Mock<IApplicationPasswordHasher>();
        hasher.Setup(x => x.HashPassword((It.IsAny<string>())))
            .Returns((string pass) => pass + "_hashed");

        var userGenerator = new Faker<TestUser>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Roles, () => new [] { Role.Customer});
        var user = userGenerator.Generate();
        
        var loggerMock = new Mock<ILogger<AccountService>>();
        var mediatorMock = new Mock<IMediator>();

        AccountService service = null;
        // Act
        Action act1 = () => service =  new AccountService(null, uowMock.Object, loggerMock.Object, mediatorMock.Object);
        act1.Should().Throw<ArgumentNullException>();
        Action act2 = () => service =  new AccountService(hasher.Object, null, loggerMock.Object, mediatorMock.Object);
        act1.Should().Throw<ArgumentNullException>();
        Action act3 = () => service =  new AccountService(hasher.Object, uowMock.Object , null, mediatorMock.Object);
        act1.Should().Throw<ArgumentNullException>();
        Action act4 = () => service =  new AccountService(hasher.Object, uowMock.Object , loggerMock.Object, null);
        act1.Should().Throw<ArgumentNullException>();
        
        

    }






}

public class TestUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role[] Roles { get; set; }
    
}