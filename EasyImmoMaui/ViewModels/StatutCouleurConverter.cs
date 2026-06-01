using System.Globalization;

namespace EasyImmoMaui.ViewModels
{
   
    public class StringNotEmptyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => !string.IsNullOrEmpty(value as string);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

   
    public class StringEmptyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

   
    public class EstUneVisiteConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is int idType && idType == 1;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // Extrait le chemin de fichier 
    public class PhotoPathConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var desc = value as string;
            if (string.IsNullOrEmpty(desc)) return null;
            if (desc.StartsWith("PHOTO:")) return desc.Substring(6);
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Convertit un libellé de statut en couleur de fond
    /// </summary>
    public class StatutCouleurFondConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string statut = (value as string)?.ToLower() ?? string.Empty;

            return statut switch
            {
                "a vendre" => Color.FromArgb("#E8F5E9"),    // Vert clair
                "sous-option" => Color.FromArgb("#FFF3E0"),    // Orange clair
                "vendu" => Color.FromArgb("#FFEBEE"),    // Rouge clair
                "en rénovation" => Color.FromArgb("#E3F2FD"),
                "a louer" => Color.FromArgb("#E0F7FA"),    // Cyan clair
                "louer" => Color.FromArgb("#B3E5FC"),      // Bleu clair (loué)
                _ => Color.FromArgb("#F5F5F5")     // Gris par défaut
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
    

    // Badge coloré pour le type de contrat 
    public class TypeContratCouleurConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value as string)?.ToLower() switch
            {
                "vente"    => Color.FromArgb("#1E40AF"),  // bleu
                "location" => Color.FromArgb("#2E7D32"),  // vert
                _          => Color.FromArgb("#666666")
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class StatutCouleurTexteConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string statut = (value as string)?.ToLower() ?? string.Empty;

            return statut switch
            {
                "a vendre" => Color.FromArgb("#2E7D32"),    // Vert foncé
                "sous-option" => Color.FromArgb("#F57C00"),    // Orange foncé
                "vendu" => Color.FromArgb("#C62828"),    // Rouge foncé
                "en rénovation" => Color.FromArgb("#1565C0"),    // Bleu foncé
                "a louer" => Color.FromArgb("#0097A7"),          // Cyan foncé
                "louer" => Color.FromArgb("#01579B"),             // Bleu marine (loué)
                _ => Color.FromArgb("#666666")
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}