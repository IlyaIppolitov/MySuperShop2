using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySuperShop.Domain.Entities;

public class Account : IEntity
{
    private string? _name;
    private string? _email;
    private string? _hashedPassword;
    private Role[]? _roles;
    

    public Account()
    {
    }

    public Account(Guid id, string name, string email, string hashedPassword, Role[] roles)
    {
        if (roles == null) throw new ArgumentNullException(nameof(roles));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(hashedPassword));

        if (!new EmailAddressAttribute().IsValid(email))
        {
            throw new AggregateException("Value is not a valid email" + email);
        }
        Id = id;
        _name = name;
        _email = email;
        _hashedPassword = hashedPassword;
        _roles = roles;
    }

    public Guid Id { get; init; }

    public string? Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is null or whitespace" + nameof(value));
            _name = value;
        }
    }

    public string? Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is null or whitespace" + nameof(value));
            if (!new EmailAddressAttribute().IsValid(value))
            {
                throw new AggregateException("Value is not a valid email" + value);
            }
            _email = value;
        }
    }

    public string? HashedPassword
    {
        get => _hashedPassword;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is null or whitespace" + nameof(value));
            _hashedPassword = value;
        }
    }

    public Role[] Roles
    {
        get => _roles;
        set => _roles = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void GrantRole(Role role)
    {
        if (!Enum.IsDefined(typeof(Role), role))
            throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(Role));
        Roles = Roles.Append(role).ToArray();
    }
}