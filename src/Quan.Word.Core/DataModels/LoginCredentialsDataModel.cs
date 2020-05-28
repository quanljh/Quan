using System;
using System.Collections.Generic;
using System.Text;

namespace Quan.Word.Core
{
    /// <summary>
    /// The data model for the login credential
    /// </summary>
    public class LoginCredentialsDataModel
    {
        /// <summary>
        /// The unique Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The users name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users login token
        /// </summary>
        public string Token { get; set; }
    }
}
