using System.Security;

namespace Quan.ViewModels.Base
{
    /// <summary>
    /// An Interface for a class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// The secure password
        /// </summary>
        SecureString SecureString { get; }
    }
}
