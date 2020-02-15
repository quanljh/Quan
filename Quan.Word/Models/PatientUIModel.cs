using Prism.Mvvm;
using System;

namespace Quan
{
    public class PatientUIModel : BindableBase
    {
        /// <summary>
        /// 患者番号
        /// </summary>
        private string _patientNo;

        public string PatientNo
        {
            get => _patientNo;
            set => SetProperty(ref _patientNo, value);
        }

        /// <summary>
        /// 患者氏名
        /// </summary>
        private string _patientName;

        public string PatientName
        {
            get => _patientName;
            set => SetProperty(ref _patientName, value);
        }

        /// <summary>
        /// フリガナ
        /// </summary>
        private string _patientKanaName;

        public string PatientKanaName
        {
            get => _patientKanaName;
            set => SetProperty(ref _patientKanaName, value);
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        private DateTime _patientBirth;

        public DateTime PatientBirth
        {
            get => _patientBirth;
            set => SetProperty(ref _patientBirth, value);
        }


        /// <summary>
        /// 性別
        /// </summary>
        private string _patientSex;

        public string PatientSex
        {
            get => _patientSex;
            set => SetProperty(ref _patientSex, value);
        }



    }
}
