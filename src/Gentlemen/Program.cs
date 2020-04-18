using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Gentlemen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseUrls("http://+:5000")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}