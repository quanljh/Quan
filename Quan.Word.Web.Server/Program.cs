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
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
