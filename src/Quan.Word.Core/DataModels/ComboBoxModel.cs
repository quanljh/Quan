using Prism.Mvvm;

namespace Quan.Word.Core
{
    public class ComboBoxModel : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        private int _value;

        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }


        /// <summary>
        /// コード
        /// </summary>}
        private string _code;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }


        /// <summary>
        /// 名前
        /// </summary>
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
