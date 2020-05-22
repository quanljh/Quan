using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        public static ApplicationDbContext ApplicationDbContext => IoCContainer.Provider.GetService<ApplicationDbContext>();
    }

    /// <summary>
    /// The dependency injection container making use of the built in .Net Core service provider
    /// </summary>
    public class IoCContainer
    {
        /// <summary>
        /// The service provider for this application
        /// </summary>
        public static IServiceProvider Provider { get; set; }

        /// <summary>
        /// The configuration manager for the application
        /// </summary>
        public static IConfiguration Configuration { get; set; }

    }
}
