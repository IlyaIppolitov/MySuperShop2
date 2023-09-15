using MySuperShop.Domain.Entities;

namespace MyShopBackend.Services;

public interface ITokenService
{
    string GenerateToken(Account account);
}