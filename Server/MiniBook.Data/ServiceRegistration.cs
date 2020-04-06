using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MiniBook.Data.Context;
using MiniBook.Data.Repositories;

namespace MiniBook.Data
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddResourceData(this IServiceCollection service, string connectionString, string dbName)
        {
            service.AddSingleton(provider =>new ResourceDbContext(connectionString, dbName));

            service.AddScoped<UserRepository>();

            return service;
        }
    }
}
