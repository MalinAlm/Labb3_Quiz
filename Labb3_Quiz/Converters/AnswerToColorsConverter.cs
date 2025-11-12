using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb3_Quiz.Converters
{
    public class AnswerToColorsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
        
            if (values.Length < 3) return Brushes.LightGray;

            var selectedAnswer = values[0] as string;
            var buttonText = values[1] as string;
            var feedbackBrush = values[2] as Brush;

            if (string.IsNullOrEmpty(selectedAnswer) || buttonText == null)
            {
                return Brushes.LightGray;
            }

            return selectedAnswer == buttonText ? feedbackBrush ?? Brushes.LightGray : Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
           => throw new NotImplementedException();
        
    }
}
