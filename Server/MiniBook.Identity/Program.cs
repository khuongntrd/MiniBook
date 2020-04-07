using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MiniBook.Identity
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(
                builder =>
                {
                    builder
#if DEBUG
                        .UseUrls("http://*:55453/")
#endif
                        .UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}