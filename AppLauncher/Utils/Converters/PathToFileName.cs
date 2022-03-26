using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AppLauncher.Utils.Converters
{
    public class PathToFileName : ConverterBase<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            return Utils.PathHelper.GetFileName(value);
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
