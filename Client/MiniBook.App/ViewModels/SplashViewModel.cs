using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MiniBook.Services;
using MiniBook.Services.Navigation;

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
