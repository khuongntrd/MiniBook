using System;
using MiniBook.Mvvm.Commands;

namespace MiniBook.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);

            Title = "Login View";
        }

        private void Login()
        {
            NavigationService.NavigateToAsync<DashboardViewModel>();
        }

        public DelegateCommand LoginCommand { get; }
        private void GoToDashboard()
        {
            NavigationService.NavigateToAsync<DashboardViewModel>();
        }
    }
}
