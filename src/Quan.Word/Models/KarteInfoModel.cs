using Prism.Mvvm;
using System;

namespace Quan.Word
{
    public class KarteInfoModel : BindableBase
    {
        private DateTime _shinryouDate;
        public DateTime ShinryouDate
        {
            get => _shinryouDate;
            set => SetProperty(ref _shinryouDate, value);
        }

        private string _doctor;
        public string Doctor
        {
            get => _doctor;
            set => SetProperty(ref _doctor, value);
        }

        private string _shinryouka;
        public string Shinryouka
        {
            get => _shinryouka;
            set => SetProperty(ref _shinryouka, value);
        }

        private string _hoken;
        public string Hoken
        {
            get => _hoken;
            set => SetProperty(ref _hoken, value);
        }

    }
}
