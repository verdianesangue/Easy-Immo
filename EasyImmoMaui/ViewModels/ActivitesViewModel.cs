using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    
    public class ActiviteAffichage
    {
        public Activite Activite { get; set; } = null!;
        public int IdActivite => Activite.IdActivite;
        public string LibelleActivite => Activite.LibelleActivite ?? string.Empty;
        public DateTime? DateActivite => Activite.DateActivite;
        public TimeOnly? DureeActivite => Activite.DureeActivite;
        public string NomBien { get; set; } = string.Empty;
        public string NomType { get; set; } = string.Empty;
    }

    public class ActivitesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ActiviteAffichage> activites = new();
        public ObservableCollection<ActiviteAffichage> Activites
        {
            get => activites;
            set { activites = value; OnPropertyChanged(); }
        }

        private string recherche = string.Empty;
        public string Recherche
        {
            get => recherche;
            set { recherche = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
