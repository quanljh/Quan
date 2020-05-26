//-------------------------------------------------------------------
// Copyright (c) 2020 EMSystems LTD. All Rights Reserved.
// ネームスペース      ：Quan.Word.Converters
// ファイル名称        ：BooleanToBorderThicknessConverter
// 新規作成者          ：EM-劉嘉豪
// 新規作成日          ：2020/05/26 14:28:10
// ファイルメモ        ：

/*-< 変更履歴 >------------------------------------------------------
*/
//-------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in a boolean and returns a thickness of 2 if true, useful for applying 
    /// border radius on a true value
    /// </summary>
    public class BooleanToBorderThicknessConverter : BaseValueConverter<bool, Thickness>
    {
        public override Thickness Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value ? new Thickness(2) : new Thickness(0);
            else
                return value ? new Thickness(0) : new Thickness(2);
        }

        public override bool ConvertBack(Thickness value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
