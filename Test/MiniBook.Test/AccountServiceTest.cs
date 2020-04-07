using MiniBook.Services;
using System;
using Xunit;

namespace MiniBook.Test
{
    public class AccountServiceTest
    {
        [Fact]
        public async void Register()
        {
            var accountService = new AccountService(new HttpService());

            var result = await accountService.RegisterAsync(new Models.User()
            {
                Firstname = "Khuong",
                Lastname = "Nguyen",
                Email = "khuongntrd@gmail.com",
                Gender = 1
            }, "Abc@123");

            Assert.True(result.Successful);
        }
        [Fact]
        public async void Login()
        {
            var accountService = new AccountService(new HttpService());

            var result = await accountService.LoginAsync("khuongntrd@outlook.com","abc!123");

            Assert.True(result);
        }
    }
}
