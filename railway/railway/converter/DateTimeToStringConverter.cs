using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace railway.converter
{
    public class DateTimeToStringConverter: MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime v = (DateTime)value;
            return v.ToString("dd.MM.yyyy.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = value as string;
            DateTime ret = DateTime.ParseExact(v, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
            if (ret != null)
            {
                return ret;
            }
            else
            {
                return value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
}
}