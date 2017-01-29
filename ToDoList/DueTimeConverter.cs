using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MyConverter {
    public class DueTimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {

            CultureInfo customCulture = new CultureInfo("fr-FR"); ;

            DateTime currentTime = DateTime.Now;

            string s = value.ToString();
            string s_curr = currentTime.ToString(customCulture);
            s_curr = s_curr.Substring(0, s_curr.Length - 3);
            string s_start = s_curr.Remove(s_curr.Length - 5, 5) + "00:00";
            string s_end = s_curr.Remove(s_curr.Length - 5, 5) + "23:59";

            DateTime dueTime = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm", customCulture);
            DateTime StartOfDayTime = DateTime.ParseExact(s_start, "dd/MM/yyyy HH:mm", customCulture);
            DateTime EndOFDayTime = DateTime.ParseExact(s_end, "dd/MM/yyyy HH:mm", customCulture); ;

            if (dueTime < currentTime)
                return "Passed";
            else if (dueTime > StartOfDayTime && dueTime < EndOFDayTime)
                return s.Substring(s.Length - 6);
            else
                return s.Substring(0, s.Length - 6);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
