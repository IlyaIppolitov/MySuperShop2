namespace MySuperShop.Domain.Repositories;

public interface IApplicationPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyHashedPassword(
        string hashedPassword, 
        string providedPassword, 
        out bool rehashNeeded);
}