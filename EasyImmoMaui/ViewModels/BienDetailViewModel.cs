using BU.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class BienDetailViewModel : INotifyPropertyChanged
    {
        // Informations générales du bien
        public string NomBien { get; set; } = string.Empty;
        public string TypeBien { get; set; } = string.Empty;
        public string StatutBien { get; set; } = string.Empty;
        public string Prix { get; set; } = string.Empty;
        public string Superficie { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Proprietaire { get; set; } = "Non renseigné";
        public string DateChangementStatut { get; set; } = string.Empty;

        // Photos du bien
        private ObservableCollection<string> photos = new();
        public ObservableCollection<string> Photos
        {
            get => photos;
            set { photos = value; NotifyPropertyChanged(); }
        }

        // Historique des activités avec participants
        private ObservableCollection<ActiviteInfo> activites = new();
        public ObservableCollection<ActiviteInfo> Activites
        {
            get => activites;
            set { activites = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
