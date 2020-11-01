namespace Quan.Word.Core
{
    /// <summary>
    /// The details to change for a User Profile from an api client call
    /// </summary>
    public class UpdateUserProfileApiModel
    {
        /// <summary>
        /// The new first name, or null to leave the same
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The new last name, or null to leave unchanged
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The new email, or null to leave unchanged
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The new username, or null to leave unchanged
        /// </summary>
        public string UserName { get; set; }
    }
}
