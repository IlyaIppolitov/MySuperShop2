using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MySuperShop.HttpApiClient;

namespace MySuperShop.Pages;

public class AppComponentBase : ComponentBase
{
    [Inject] protected IMyShopClient Client { get; private set; }
    [Inject] protected ILocalStorageService LocalStorage { get; private set; }
    [Inject] protected AppState State { get; private set; }
    protected bool IsTokenChecked { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (State.IsTokenChecked) return;
        
        var token = await LocalStorage.GetItemAsync<string>("token");
        if (!string.IsNullOrEmpty(token))
        {
            Client.SetAuthorizationToken(token);
            State.IsTokenChecked = true;
        }
    }
}