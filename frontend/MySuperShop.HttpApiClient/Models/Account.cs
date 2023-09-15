using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Account
{
    private Guid _id;
    private string? _name;
    private string? _email;
    private Role[]? _roles;

    public Account()
    {
    }

    public Account(string name, string email, Role[] roles)
    {
        if (roles == null) throw new ArgumentNullException(nameof(roles));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

        if (!new EmailAddressAttribute().IsValid(email))
        {
            throw new AggregateException("Value is not a valid email" + email);
        }
        Id = Guid.NewGuid();
        _name = name;
        _email = email;
        _roles = roles;
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

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

    public string RolesString
    {
        get
        {
            return string.Join(",", Roles);
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value is null or whitespace" + nameof(value));
            Roles = value.Split(',').Select(Enum.Parse<Role>).ToArray();
        }
    }
}

public enum Role
{
    Customer, Manager, Admin
}