using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Quan.AspNet;

namespace Quan.Word.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder()
                // Add Quan Framework
                .UseQuanFramework(construct =>
                {
                    // Configure framework

                    // Add file logger
                    construct.AddFileLogger();
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
