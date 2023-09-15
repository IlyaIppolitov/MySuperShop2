using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyShopBackend.Configurations;
using MySuperShop.Domain.Entities;

namespace MyShopBackend.Services;


public class TokenService : ITokenService
{
    private readonly JwtConfig _jwtConfig;

   public TokenService(JwtConfig jwtConfig)
    {
        if (jwtConfig == null) throw new ArgumentNullException(nameof(jwtConfig));
        _jwtConfig = jwtConfig;
    }
    
    public string GenerateToken(Account account)
    {
        if (account == null) throw new ArgumentNullException(nameof(account));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = CreateClaimsIdentity(account),
            Expires = DateTime.UtcNow.Add(_jwtConfig.LifeTime),
            Audience = _jwtConfig.Audience,
            Issuer = _jwtConfig.Issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private ClaimsIdentity CreateClaimsIdentity(Account account)
    {
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
        });
        foreach (var role in account.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
        }
        return claimsIdentity;
    }

}