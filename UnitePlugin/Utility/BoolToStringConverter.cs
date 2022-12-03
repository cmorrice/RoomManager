using System;
using System.Globalization;
using System.Windows.Data;

namespace UnitePlugin.Utility
{
    [Serializable]
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueText { get; set; }
        public string FalseText { get; set; }
        
        public BoolToStringConverter(string trueText, string falseText)
        {
            TrueText = trueText;
            FalseText = falseText;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool)) return null;
            var bVal = (bool)value;
            return bVal ? this.TrueText: this.FalseText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string)) return null;
            var sVal = (string)value;
            
            if (sVal == TrueText) return true;
            if (sVal == FalseText) return false;
            return null;
        }

        public string Convert(bool value)
        {
            return (string)Convert(value, typeof(string), null, CultureInfo.CurrentUICulture);
        }
    }
}