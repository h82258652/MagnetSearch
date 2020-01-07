using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace MagnetSearch.Uwp.Converters
{
    public class TimeSpanToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is TimeSpan timeSpan))
            {
                return null;
            }

            var buffer = new StringBuilder();
            if (timeSpan.Hours > 0)
            {
                buffer.Append(timeSpan.Hours + "小时");
            }

            if (timeSpan.Minutes > 0)
            {
                buffer.Append(timeSpan.Minutes + "分钟");
            }

            buffer.Append(timeSpan.Seconds + "秒");

            return buffer.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
