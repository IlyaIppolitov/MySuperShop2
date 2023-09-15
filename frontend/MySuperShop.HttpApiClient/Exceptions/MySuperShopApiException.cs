using System.Net;
using MySuperShop.HttpModels.Responses;

namespace MySuperShop.HttpApiClient.Exceptions;

[Serializable]
public class MySuperShopApiException : Exception
{
    public ErrorResponse? Error { get; }
    public ValidationProblemDetails? Details { get; }
    public HttpStatusCode? StatusCode { get; }

    public MySuperShopApiException()
    {
    }

    public MySuperShopApiException(HttpStatusCode statusCode, ValidationProblemDetails details) : base(details.Title)
    {
        StatusCode = statusCode;
        Details = details;
    }

    public MySuperShopApiException(ErrorResponse error) : base(error.Message)
    {
        Error = error;
        StatusCode = error.StatusCode;
    }

    public MySuperShopApiException(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
    }

    public MySuperShopApiException(string? message) : base(message)
    {
    }

    public MySuperShopApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}