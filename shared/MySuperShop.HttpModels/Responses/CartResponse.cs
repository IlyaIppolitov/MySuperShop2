namespace MySuperShop.HttpModels.Responses;

public record CartResponse
{
    public List<ItemResponse> Items { get; set; } = new List<ItemResponse>();
};
public record ItemResponse (Guid Id, string ProductName, double Quantity);