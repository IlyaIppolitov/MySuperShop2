using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyShopBackend.Filters;

public class AuthentificationRequestFilter : IAuthorizationFilter
{
    private readonly ILogger<AuthentificationRequestFilter> _logger;

    public AuthentificationRequestFilter(
        ILogger<AuthentificationRequestFilter> logger
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}