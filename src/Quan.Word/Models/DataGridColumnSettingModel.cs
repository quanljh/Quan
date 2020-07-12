using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Quan.Word
{
    public class DataGridColumnSettingModel
    {
        public bool IsDisplay { get; set; }

        public DataGridLength Width { get; set; }

        public int DisplayIndex { get; set; }

        public DataGridColumnSettingModel(bool isDisplay, double width, int displayIndex)
        {
            IsDisplay = isDisplay;
            Width = new DataGridLength(width);
            DisplayIndex = displayIndex;
        }

        public static ObservableCollection<DataGridColumnSettingModel> GetDefaultDataGridColumnSettings()
        {
            var settings = new ObservableCollection<DataGridColumnSettingModel>();

            var displayIndex = 0;
            settings.Add(new DataGridColumnSettingModel(true, 100, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(false, 120, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 120, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 150, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 60, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 100, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 100, displayIndex++));
            settings.Add(new DataGridColumnSettingModel(true, 170, displayIndex));

            return settings;
        }
    }
}
