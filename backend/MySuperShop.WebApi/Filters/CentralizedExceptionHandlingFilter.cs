using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySuperShop.Domain.Exceptions;
using MySuperShop.HttpModels.Responses;

namespace MyShopBackend.Filters;

public class CentralizedExceptionHandlingFilter 
    : Attribute, IExceptionFilter, IOrderedFilter
{
    public int Order { get; set; }

    public void OnException(ExceptionContext context)
    {
        var message = TryGetUserMessageFromException(context);
        HttpStatusCode statusCode = HttpStatusCode.Conflict;
        if (message != null)
        {
            context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
            {
                StatusCode=(409)
            };
            context.ExceptionHandled = true;
        }
    }

    private string? TryGetUserMessageFromException(ExceptionContext context)
    {
        return context.Exception switch
        {
            AccountNotFoundException => "Аккаунт с таким Email не найден",
            EmailAlreadyExistsException => "Аккаунт с таким Email уже зарегистрирован!",
            InvalidPasswordException => "Неверный пароль",
            CodeNotFoundException => "Код не генерировался для данного аккаунта!",
            InvalidCodeException => "Код плохой!",
            DomainException => "Необработанная ошибка!",
            _ => null
        };
    }
    
}