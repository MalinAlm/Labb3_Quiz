
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb3_Quiz.Converters
{
    public class AnswerToColorsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
        
            if (values.Length < 4) return Brushes.LightGray;

            var selectedAnswer = values[0] as string;
            var buttonText = values[1] as string;
            var correctAnswer = values[2] as string;
            var feedbackBrush = values[2] as Brush;

            if (string.IsNullOrEmpty(selectedAnswer)) return Brushes.LightGray;

            if (buttonText == correctAnswer) return Brushes.LightGreen;

            if (buttonText == selectedAnswer) return Brushes.Red;

            return Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
           => throw new NotImplementedException();
    }
}
