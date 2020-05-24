using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Quan.Word.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
            // provides default configuration for the app such as appsettings.json and appsettings.{Environment}.json using the JSON configuration provider
            // Secret Manager when the app runs in the Development environment
            // Environment variables
            // Command-line arguments
            // Add logging providers
            return WebHost.CreateDefaultBuilder()
                // Specify the startup type to be used by the web host
                .UseStartup<Startup>()
                .Build();
        }
    }
}
