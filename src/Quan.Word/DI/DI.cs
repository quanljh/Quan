using Quan.Word.Core;

namespace Quan.Word
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// A shortcut to access the <see cref="IUImanager"/>
        /// </summary>
        public static IUImanager UI => Framework.Service<IUImanager>();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel ApplicationVM => Framework.Service<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="SettingsViewModel"/>
        /// </summary>
        public static SettingsViewModel SettingsVM => Framework.Service<SettingsViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="IClientDataStore"/> service
        /// </summary>
        public static IClientDataStore ClientDataStore => Framework.Service<IClientDataStore>();
    }
}
