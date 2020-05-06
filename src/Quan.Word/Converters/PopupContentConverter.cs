
using Quan.Word.Core;
using System;
using System.Globalization;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in a <see cref="ViewModelBase"/>and returns the specific UI control
    /// that should bind to that type of ViewModel
    /// </summary>
    public class PopupContentConverter : BaseValueConverter<ChatAttachmentPopupMenuViewModel, object>
    {
        public override object Convert(ChatAttachmentPopupMenuViewModel value, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return new VerticalMenu { DataContext = value.Content };
        }

        public override ChatAttachmentPopupMenuViewModel ConvertBack(object value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
