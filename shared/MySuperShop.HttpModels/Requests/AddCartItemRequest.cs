namespace MySuperShop.HttpModels.Requests;

public record AddCartItemRequest(Guid AccountId, Guid ProductId, double Quantity);