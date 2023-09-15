using System.Collections.Concurrent;
using MySuperShop.HttpModels.Requests;
using MySuperShop.HttpModels.Responses;

namespace MySuperShop.HttpApiClient
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken = default);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken = default);
        Task<Product[]> GetProducts(CancellationToken cancellationToken = default);
        Task UpdateProduct(Product product, CancellationToken cancellationToken = default);
        Task DeleteProduct(Product product, CancellationToken cancellationToken = default);
        Task<LoginByCodeResponse> Register(RegisterRequest account, CancellationToken cancellationToken = default);
        Task<LoginByCodeResponse> Login(LoginByPassRequest request, CancellationToken cancellationToken = default);
        Task<ConcurrentDictionary<string, int>> GetMetrics(CancellationToken cancellationToken = default);
        Task<Account[]> GetAccounts(CancellationToken cancellationToken = default);
        Task<AccountResponse> GetCurrentAccount(CancellationToken cancellationToken = default);
        Task<UpdateAccountResponse> UpdateAccount(UpdateAccountRequest account, CancellationToken cancellationToken = default);
        void SetAuthorizationToken(string token);
        Task AddCartItemToCart(AddCartItemRequest request, CancellationToken cancellationToken = default);
        Task<CartResponse> GetCart(CancellationToken cancellationToken = default);
    }
}