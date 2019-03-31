using System;
using MiniBook.Mvvm.Commands;

namespace MiniBook.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);
            Title = "Login View";
        }

        private void Register()
        {
            NavigationService.NavigateToAsync<RegisterViewModel>();
        }

        private void Login()
        {
            NavigationService.NavigateToAsync<DashboardViewModel>();
        }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand LoginCommand { get; }
        private void GoToDashboard()
        {
            NavigationService.NavigateToAsync<DashboardViewModel>();
        }
    }
}
