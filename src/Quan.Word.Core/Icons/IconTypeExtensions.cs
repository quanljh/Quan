namespace Quan.Word.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class IconTypeExtensions
    {
        /// <summary>
        /// Converts <see cref="IconType"/> to a FontAwesome string
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <returns></returns>
        public static string ToFontAwesome(this IconType type)
        {
            //Return a FontAwesome string based on the icon type
            switch (type)
            {
                case IconType.File:
                    return "\xf0f6";

                case IconType.Picture:
                    return "\xf1c5";

                //if none found,return null
                default:
                    return null;
            }
        }
    }
}
