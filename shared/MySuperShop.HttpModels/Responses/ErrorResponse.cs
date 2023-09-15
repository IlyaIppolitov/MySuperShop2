using System.Net;

namespace MySuperShop.HttpModels.Responses;

public record ErrorResponse(string Message, HttpStatusCode? StatusCode = null);