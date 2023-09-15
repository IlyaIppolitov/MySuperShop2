using Microsoft.AspNetCore.Identity;
using MySuperShop.Domain.Repositories;

namespace IdentityPasswordHasherLib;

public class IdentityPasswordHasher : IApplicationPasswordHasher
{

    private readonly PasswordHasher<object> _passwordHasher = new();
    private readonly object _fake = new();
    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        return _passwordHasher.HashPassword(_fake, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool rehashNeeded)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);
        
        var result = _passwordHasher.VerifyHashedPassword(_fake, hashedPassword, providedPassword);
        rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
        return  result != PasswordVerificationResult.Failed;
    }
}