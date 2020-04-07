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
        public static void AddResourceData(this IServiceCollection services, string connectionString, string dbName)
        {
            services.AddSingleton(s => new ResourceDbContext(connectionString, dbName));
            services.AddScoped<UserRepository>();
            services.AddScoped<PostRepository>();
            services.AddScoped<FeedRepository>();
        }
    }
}
