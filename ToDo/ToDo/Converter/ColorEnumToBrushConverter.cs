using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDo.Converter
{
    public class ColorEnumToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Entities.Color color)
            {
                switch (color)
                {
                    case Entities.Color.Blue:
                        return Brushes.Blue;
                    case Entities.Color.Green:
                        return Brushes.Green;
                    case Entities.Color.Purple:
                        return Brushes.Purple;
                    case Entities.Color.Orange:
                        return Brushes.Orange;
                    case Entities.Color.Red:
                        return Brushes.Red;
                    case Entities.Color.Yellow:
                        return Brushes.Yellow;
                    default:
                        return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO: If needed, implement
            throw new NotImplementedException();
        }
    }
}
