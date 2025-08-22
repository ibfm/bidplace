using BidPlace.ClientApp.Services;
using BidPlace.ClientApp.ViewModels.Base;

namespace BidPlace.ClientApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService)
        : base(navigationService)
    {
    }

    [RelayCommand]
    private async Task SettingsAsync()
    {
        await NavigationService.NavigateToAsync("Settings");
    }
}
