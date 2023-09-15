using Microsoft.AspNetCore.Components;
using MudBlazor;
using MySuperShop.HttpApiClient.Exceptions;
using MySuperShop.HttpModels.Requests;

namespace MySuperShop.Pages
{
    public partial class RegistrationPage
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private RegisterRequest _model = new RegisterRequest();
        private bool _registrationInProgress = false;

        private async Task ProcessRegistration()
        {
            if (_registrationInProgress)
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Пожалуйста, подождите...", Severity.Info);
                return;
            }
            _registrationInProgress = true;
            try
            {
                var response = await Client.Register(_model);
                await LocalStorage.SetItemAsync("token", response.Token);
                State.IsTokenChecked = true;
                await DialogService.ShowMessageBox(
                    "Успех!",
                    $"Вы успешно зарегистрировались! Молодец!",
                    yesText: "Ok!");
                NavigationManager.NavigateTo("/account/current");
            }
            catch (MySuperShopApiException ex)
            {
                _registrationInProgress = false;
                await DialogService.ShowMessageBox(
                    "Ошибка!",
                    $"Ошибка регистрации: {ex.Message}");
            }
            finally
            {
                _registrationInProgress = false;
            }
        }
    }
    
    
}
