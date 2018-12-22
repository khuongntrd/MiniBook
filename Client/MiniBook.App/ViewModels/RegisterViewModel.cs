using MiniBook.Models;
using MiniBook.Mvvm.Commands;
using MiniBook.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
            await AccountService.RegisterAsync(User, Password);
        }

        private bool CanRegister()
        {
            return true;
        }
    }
}
