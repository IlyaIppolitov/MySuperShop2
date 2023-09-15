using System.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MySuperShop.HttpApiClient.Exceptions;
using MySuperShop.HttpModels.Requests;

namespace MySuperShop.Pages
{
	public partial class CatalogPage
	{
		[Inject] private IDialogService DialogService { get; set; }
		private Product[]? _products;
		private CancellationTokenSource _cts = new();
		private bool _editPopoverIsOpen = false;
		private bool _quantityInputPopoverIsOpen = false;
		private string _name = "";
		private string _pic = "";
		private string _description = "";
		private decimal _price = 0;
		private double _stock = 0;
		private double _quantity = 1;
		private Guid _currentAccountId;
		private Guid _currentProductId;

		protected override async Task OnInitializedAsync()
		{
			await GetProducts();
		}

		void NavToProductCard(Guid guid)
		{
			Navigation.NavigateTo($"/products/{guid.ToString()}");
		}

		private async Task DeleteProduct(Product product)
		{
			bool? result = await DialogService.ShowMessageBox(
				"Warning", 
				"Deleting can not be undone!", 
				yesText:"Delete!", cancelText:"Cancel");
			if (result is not null && result.Value)
			{
				await Client.DeleteProduct(product);
				await GetProducts();
				await InvokeAsync(() => StateHasChanged());
			}
		}

		private async Task GetProducts()
		{
			_products = await Client.GetProducts(_cts.Token);
		}

		void Dispose()
		{
			// Выполнится в момент закрытия/переключения страницы
			_cts.Cancel();
		}

		private void CreateProduct()
		{
			_editPopoverIsOpen = true;
		}

		private async Task CloseEditPopover()
		{
			_editPopoverIsOpen = false;
		}

		private async Task SaveAndCloseEditPopover()
		{
			if (string.IsNullOrWhiteSpace(_name))
			{
				await DialogService.ShowMessageBox(
					"Warning", 
					"Введите наименование!");
			}
			else
			{
				await Client.AddProduct(new Product(Guid.NewGuid(), _name, _description, _price, _stock, _pic));
				await GetProducts();
				await InvokeAsync(() => StateHasChanged());
				await CloseEditPopover();
			}
		}

		private async Task AddProductToCart(Product product)
		{
			_currentProductId = product.Id;
			_quantityInputPopoverIsOpen = true;
		}

		private void ClosQuantityInputPopover()
		{
			_quantityInputPopoverIsOpen = false;
		}

		private async Task ConfirmAndCloseQuantityInputPopover()
		{
			try
			{
				var resp = await Client.GetCurrentAccount();
				_currentAccountId = resp.Id;

				await Client.AddCartItemToCart(new AddCartItemRequest(_currentAccountId, _currentProductId, _quantity));
			
			}
			catch (MySuperShopApiException ex)
			{
				await DialogService.ShowMessageBox(
					"Ошибка!",
					$"Ошибка авторизации: {ex.Message}");
			}

			_quantity = 1;
			_quantityInputPopoverIsOpen = false;
		}
	}
}
