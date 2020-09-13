using Quan.Word.Core;
using System;

namespace Quan.Word
{
    public class TextBoxPageViewModel : ViewModelBase
    {
        #region Properties

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (SetProperty(ref _text, value))
                    ValidateText();
            }
        }



        #endregion

        #region Commands

        #endregion

        #region Constructor

        public TextBoxPageViewModel()
        {
        }



        #endregion

        #region Method

        private void ValidateText()
        {
            ClearErrors(nameof(Text));
            if (string.IsNullOrWhiteSpace(Text))
                AddError(nameof(Text), "Username cannot be empty.");
            if (string.Equals(Text, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(Text), "Admin is not valid username.");
            if (Text == null || Text?.Length <= 5)
                AddError(nameof(Text), "Username must be at least 6 characters long.");
        }

        #endregion
    }
}



