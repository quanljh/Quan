using System.ComponentModel.DataAnnotations;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// Our settings database table representational model
    /// </summary>
    public class SettingsDataModel
    {
        /// <summary>
        /// The unique Id for this entry
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The settings name
        /// </summary>
        /// <remarks>This column is index</remarks>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// The settings value
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public string Value { get; set; }
    }
}
