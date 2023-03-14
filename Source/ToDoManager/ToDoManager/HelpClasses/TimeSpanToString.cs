using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoManager.HelpClasses
{
    public class TimeSpanToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.Parse(value.ToString());
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            TimeSpan time = (TimeSpan)value;
            return time.ToString(@"hh\:mm");
        }
    }
}
