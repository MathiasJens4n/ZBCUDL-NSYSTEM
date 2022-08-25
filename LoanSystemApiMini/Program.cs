using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LoanSystemApiMini
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://10.125.169.3:26546")
                    .UseStartup<Startup>();
                });
        }
    }
}
