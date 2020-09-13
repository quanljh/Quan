using Microsoft.Extensions.DependencyInjection;
using Quan.Word.Core;

namespace Quan.Word
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Injects the view models needed for Quan Word application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddQuanWordViewModels(this FrameworkConstruction construction)
        {
            // Bind to a single instance of Application view model
            construction.Services.AddSingleton<ApplicationViewModel>();

            // Bind to a single instance of Settings view model
            construction.Services.AddSingleton<SettingsViewModel>();

            // Return the construction from chaining
            return construction;
        }

        /// <summary>
        /// Injects the Quan Word client application services needed
        /// for the Quan word application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddQuanWordClientServices(this FrameworkConstruction construction)
        {
            // Add our task manager
            construction.Services.AddTransient<ITaskManager, BaseTaskManager>();

            // Add out file manager
            construction.Services.AddTransient<IFileManager, BaseFileManager>();

            // Add our UI Manager
            construction.Services.AddTransient<IUImanager, UIManager>();

            // Return the construction from chaining
            return construction;
        }
    }
}
