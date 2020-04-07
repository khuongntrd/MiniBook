using MiniBook.Services;
using MiniBook.Services.Navigation;
using System.Threading.Tasks;

namespace MiniBook.ViewModels
{
    public class SplashViewModel : ViewModelBase
    {
        public override async Task OnNavigationAsync(NavigationParameters parameters, NavigationType navigationType)
        {
            if (await ServiceLocator.Instance.Resolve<AccountService>().RestoreAsync())
            {
                await NavigationService.NavigateToAsync<DashboardViewModel>();
            }
            else
            {
                await NavigationService.NavigateToAsync<LoginViewModel>();
            }
        }
    }
}
