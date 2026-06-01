using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class DocumentsViewModel : INotifyPropertyChanged
    {
        public string TitreBien { get; set; } = string.Empty;

        // Photos du bien 
        private ObservableCollection<DocumentBien> photos = new();
        public ObservableCollection<DocumentBien> Photos
        {
            get => photos;
            set { photos = value; NotifyPropertyChanged(); }
        }

        // Documents texte et PDF
        private ObservableCollection<DocumentBien> documents = new();
        public ObservableCollection<DocumentBien> Documents
        {
            get => documents;
            set { documents = value; NotifyPropertyChanged(); }
        }

        // Texte pour ajouter une description manuelle
        private string nouveauDocument = string.Empty;
        public string NouveauDocument
        {
            get => nouveauDocument;
            set { nouveauDocument = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
