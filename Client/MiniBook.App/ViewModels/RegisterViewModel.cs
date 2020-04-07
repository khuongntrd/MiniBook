using MiniBook.Models;
using MiniBook.Mvvm.Commands;
using MiniBook.Services;

namespace MiniBook.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private AccountService AccountService { get; }

        public RegisterViewModel(AccountService accountService)
        {
            AccountService = accountService;

            RegisterCommand = new DelegateCommand(Register, CanRegister);
        }

        public User User { get; set; } = new User();

        public string Password { get; set; }

        public DelegateCommand RegisterCommand { get; }

        private async void Register()
        {
            var result = await AccountService.RegisterAsync(User, Password);
            if (result.Successful)
                await NavigationService.NavigateBackAsync();
        }

        private bool CanRegister()
        {
            return true;
        }
    }
}
