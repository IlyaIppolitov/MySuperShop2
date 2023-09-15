namespace MySuperShop.HttpModels.Responses;

public record AccountResponse(Guid Id, string Name, string Email, string Roles);