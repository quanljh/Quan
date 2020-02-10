using Ninject;

namespace Quan.Word.Core
{
    /// <summary>
    /// The IoC container for our application
    /// </summary>
    public static class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="IUImanager"/>
        /// </summary>
        public static IUImanager UI => IoC.Get<IUImanager>();

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the IoC container binds all information required and is ready for use
        /// Note: Must be called as soon as your application starts up to ensure all
        ///       service can be found
        /// 
        /// </summary>
        public static void SetUp()
        {
            //Binding all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view modles
        /// </summary>
        private static void BindViewModels()
        {
            //Bind to a single instance of Application view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }

        #endregion


        /// <summary>
        /// Gets a service from the IoC,of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
