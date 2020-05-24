using System.Security;

namespace Quan.Word.Core
{
    /// <summary>
    /// An Interface for a class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// The secure password
        /// </summary>
        SecureString SecurePassword { get; }
    }
}
