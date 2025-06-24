using System.Globalization;

namespace AppRpgEtec.Converters
{
    /// <summary>
    /// Converte o valor de pontos de vida em uma cor (verde, laranja, vermelha).
    /// </summary>
    public class PontosVidaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pontos)
            {
                if (pontos >= 50)
                    return Colors.Green;
                else if (pontos >= 20)
                    return Colors.Orange;
                else
                    return Colors.Red;
            }

            return Colors.Gray; // fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack não é necessário para PontosVidaColorConverter.");
        }
    }
}
