using System.Globalization;

namespace EasyImmoMaui.ViewModels
{
    /// <summary>
    /// Convertit menuActif en couleur 
    /// </summary>
    public class MenuActifConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string menuActif = value as string ?? string.Empty;
            string nomMenu = parameter as string ?? string.Empty;

            return menuActif == nomMenu ? Color.FromArgb("#FF8C00") : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}