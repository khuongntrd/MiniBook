using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniBook.Data;
using Newtonsoft.Json;

namespace MiniBook.Resource
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(builder => builder.MapDefaultControllerRoute());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(
                    options =>
                    {
                        options.Authority = Configuration["Identity:Authority"];
                        options.RequireHttpsMetadata = false;
                    });

            services.AddResourceData(
                Configuration["Data:ConnectionString"],
                Configuration["Data:DbName"]);

            services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });
        }
    }
}