using System;
using Windows.UI.Xaml.Data;

namespace ShopNow.Common
{
    public sealed class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var price = System.Convert.ToDouble(value);
            return price.ToString("$#,###.00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return 0;
        }
    }
}