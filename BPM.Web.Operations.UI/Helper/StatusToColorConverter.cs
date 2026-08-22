using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BPM.Web.Operations.UI.Helper
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "draft" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94a3b8")),
                    "submitted" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3b82f6")),
                    "approved" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22c55e")),
                    "received" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0d9488")),
                    "cancelled" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ef4444")),
                    _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94a3b8"))
                };
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94a3b8"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}