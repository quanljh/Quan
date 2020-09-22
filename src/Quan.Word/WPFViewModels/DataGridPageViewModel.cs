using Quan.Word.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Quan.Word
{
    public class DataGridPageViewModel : ViewModelBase
    {
        #region Properties

        public DataGridPage DataGridPage;

        public ObservableCollection<JyokyoUIModel> JokyoCollection { get; set; }

        public ObservableCollection<PatientUIModel> PatientCollection { get; set; }

        public ListCollectionView PatientCollectionView { get; set; }

        private PatientUIModel _selectedPatient;

        public PatientUIModel SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        public ObservableCollection<DataGridColumnSettingModel> DataGridColumnSettings { get; set; } =
            DataGridColumnSettingModel.GetDefaultDataGridColumnSettings();

        private bool _flag;

        public bool Flag
        {
            get => _flag;
            set => SetProperty(ref _flag, value);
        }

        #endregion

        #region Commands

        public ICommand AsyncCommand { get; set; }

        public ICommand Async2Command { get; set; }
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

            var karteCollection = new ObservableCollection<KarteInfoModel>()
            {
                new KarteInfoModel()
                {
                    ShinryouDate = DateTime.Now,
                    Doctor = "藤原拓海",
                    Shinryouka = "内科",
                    Hoken = "医保"
                },
                new KarteInfoModel()
                {
                    ShinryouDate = DateTime.Now,
                    Doctor = "鈴木玲奈",
                    Shinryouka = "外科",
                    Hoken = "国保"
                },
                new KarteInfoModel()
                {
                    ShinryouDate = DateTime.Now,
                    Doctor = "出川哲朗",
                    Shinryouka = "小児科",
                    Hoken = "自賠責"
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
                    PatientJoukyouKbn = "1",
                    PatientAddress = "大阪",
                    KarteCollection = karteCollection
                },
                new PatientUIModel()
                {
                    PatientNo = "2",
                    PatientName = "山柴典隆",
                    PatientKanaName = "ﾔﾏｼﾊﾞﾉﾘﾀｶ",
                    PatientBirth = new DateTime(1973,02,01),
                    PatientSex = "1",
                    PatientJoukyouKbn = "2",
                    PatientAddress = "大阪",
                    KarteCollection = new ObservableCollection<KarteInfoModel>()
                    {
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "設楽統",
                            Shinryouka = "内科",
                            Hoken = "医保"
                        },
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "日村勇紀",
                            Shinryouka = "脳科",
                            Hoken = "公費"
                        },
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "秋元真夏",
                            Shinryouka = "心臓血管外科",
                            Hoken = "介護"
                        }
                    }
                },
                new PatientUIModel()
                {
                    PatientNo = "3",
                    PatientName = "米山亮",
                    PatientKanaName = "ﾖﾈﾔﾏｱｷﾗ",
                    PatientBirth = new DateTime(1983,04,15),
                    PatientSex = "1",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪",
                    KarteCollection = new ObservableCollection<KarteInfoModel>()
                    {
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "橋本奈々未",
                            Shinryouka = "内科",
                            Hoken = "医保"
                        },
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "白石麻衣",
                            Shinryouka = "脳科",
                            Hoken = "公費"
                        },
                        new KarteInfoModel()
                        {
                            ShinryouDate = DateTime.Now,
                            Doctor = "生田絵梨花",
                            Shinryouka = "心臓血管外科",
                            Hoken = "介護"
                        }
                    }
                },
                new PatientUIModel()
                {
                    PatientNo = "4",
                    PatientName = "金山桜子",
                    PatientKanaName = "ｶﾅﾔﾏｻｸﾗｺ",
                    PatientBirth = new DateTime(1988,03,22),
                    PatientSex = "2",
                    PatientJoukyouKbn = "4",
                    PatientAddress = "大阪"
                },
                new PatientUIModel()
                {
                    PatientNo = "5",
                    PatientName = "田中稚香5",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪"
                },
                new PatientUIModel()
                {
                    PatientNo = "6",
                    PatientName = "田中稚香6",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪"
                },
                new PatientUIModel()
                {
                    PatientNo = "7",
                    PatientName = "田中稚香7",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪"
                },
                new PatientUIModel()
                {
                    PatientNo = "8",
                    PatientName = "田中稚香8",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪"
                },
                new PatientUIModel()
                {
                    PatientNo = "9",
                    PatientName = "田中稚香9",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986,05,02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "大阪"
                },
            };

            for (int i = 1; i < 100; i++)
            {
                PatientCollection.Add(new PatientUIModel()
                {
                    PatientNo = $"{i + 9}",
                    PatientName = $"田中稚香{i + 9}",
                    PatientKanaName = "ﾀﾅｶﾁｶ",
                    PatientBirth = new DateTime(1986, 05, 02),
                    PatientSex = "2",
                    PatientJoukyouKbn = "3",
                    PatientAddress = "東京"
                });
            }

            PatientCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(PatientCollection);

            PatientCollectionView.IsLiveGrouping = true;

            SelectedPatient = PatientCollection.FirstOrDefault();

            AsyncCommand = new DelegateAsyncCommand(async () => await TestAsync()).ObservesCanExecute(() => Flag);
            AsyncCommand = new DelegateAsyncCommand(async () => await Test2Async()).ObservesCanExecute(() => Flag);

            PatientCollectionView.GroupDescriptions?.Add(new PropertyGroupDescription("PatientAddress"));

            PatientCollectionView.LiveGroupingProperties.Add("PatientAddress");


            //patientCollectionView.Filter = x =>
            //{
            //    if (!(x is PatientUIModel patient))
            //        return false;
            //    return patient.PatientNo != "4";
            //};
        }


        #endregion

        #region Method

        private async Task TestAsync()
        {
            await Task.Delay(5000);
        }

        private async Task Test2Async()
        {
            await Task.Delay(5000);
        }

        #endregion
    }
}



