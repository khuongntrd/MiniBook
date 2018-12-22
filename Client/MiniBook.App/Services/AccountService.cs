using MiniBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniBook.Services
{
    public class AccountService
    {
        HttpService HttpService { get; }

        public AccountService(HttpService httpService)
        {
            HttpService = httpService;
        }

        public Task<ApiResponse<object>> RegisterAsync(User user, string password)
        {
            var url = Configuration.ID_HOST + "/api/account";

            return HttpService.PostAsync<object>(url, new
            {
                user.Firstname,
                user.Lastname,
                user.Gender,
                user.Email,
                user.BirthDate,
                Password = password
            });
        }
    }
}
