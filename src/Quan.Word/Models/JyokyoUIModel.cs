using Prism.Mvvm;

namespace Quan.Word
{
    public class JyokyoUIModel : BindableBase
    {
        private string _jyokyoName;

        public string JyokyoName
        {
            get => _jyokyoName;
            set => SetProperty(ref _jyokyoName, value);
        }

        private string _jyokyoCode;

        public string JyokyoCode
        {
            get => _jyokyoCode;
            set => SetProperty(ref _jyokyoCode, value);
        }


    }
}
