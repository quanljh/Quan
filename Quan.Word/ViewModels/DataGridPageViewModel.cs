﻿using Prism.Mvvm;
using Quan.Word.Core;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Quan
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

    public class DataGridPageViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<JyokyoUIModel> JokyoCollection { get; set; }

        public ObservableCollection<PatientUIModel> PatientCollection { get; set; }

        #endregion

        #region Commands

        public ReactiveCommand ChangeStatesCommand { get; set; }

        #endregion

        #region Constructor

        public DataGridPageViewModel()
        {
            JokyoCollection = new ObservableCollection<JyokyoUIModel>()
            {
                new JyokyoUIModel()
                {
                    JyokyoCode = "1",
                    JyokyoName = "来院待ち"
                },
                new JyokyoUIModel()
                {
                    JyokyoCode = "2",
                    JyokyoName = "診察待ち"
                },
                new JyokyoUIModel()
                {
                    JyokyoCode = "3",
                    JyokyoName = "診察中"
                },
                new JyokyoUIModel()
                {
                    JyokyoCode = "4",
                    JyokyoName = "会計待ち"
                }
            };

            PatientCollection = new ObservableCollection<PatientUIModel>()
            {
                new PatientUIModel()
                {
                    PatientNo = "1",
                    PatientName = "安倍晋三",
                    PatientKanaName = "ｱﾍﾞｼﾝｿﾞｳ",
                    PatientBirth = new DateTime(1965,07,05),
                    PatientSex = "1",
                    PatientJoukyouKbn = "1"
                },
                new PatientUIModel()
                {
                    PatientNo = "2",
                    PatientName = "山柴典隆",
                    PatientKanaName = "ﾔﾏｼﾊﾞﾉﾘﾀｶ",
                    PatientBirth = new DateTime(1973,02,01),
                    PatientSex = "1",
                    PatientJoukyouKbn = "2"
                },
                new PatientUIModel()
                {
                    PatientNo = "3",
                    PatientName = "米山亮",
                    PatientKanaName = "ﾖﾈﾔﾏｱｷﾗ",
                    PatientBirth = new DateTime(1983,04,15),
                    PatientSex = "1",
                    PatientJoukyouKbn = "3"
                },
                new PatientUIModel()
                {
                    PatientNo = "4",
                    PatientName = "金山桜子",
                    PatientKanaName = "ｶﾅﾔﾏｻｸﾗｺ",
                    PatientBirth = new DateTime(1988,03,22),
                    PatientSex = "0",
                    PatientJoukyouKbn = "4"
                },
                new PatientUIModel()
                {
                    PatientNo = "5",
                    PatientName = "田中稚香",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "0",
                    PatientJoukyouKbn = "3"
                },
            };

            ChangeStatesCommand = new ReactiveCommand();

            ChangeStatesCommand.Subscribe(async () => await wait());
        }

        #endregion

        #region Method

        public async Task wait()
        {
            await Task.Delay(3000);
        }

        #endregion
    }
}



