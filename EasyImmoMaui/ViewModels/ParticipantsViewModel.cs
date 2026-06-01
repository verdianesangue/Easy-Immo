using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ParticipantsViewModel : INotifyPropertyChanged
    {
        public string TitreActivite { get; set; } = string.Empty;

        // Liste des personnes qui participent déjà
        private ObservableCollection<Personne> participants = new();
        public ObservableCollection<Personne> Participants
        {
            get => participants;
            set { participants = value; OnPropertyChanged(); }
        }

        // Liste des personnes qu'on peut ajouter 
        private ObservableCollection<Personne> personnesDisponibles = new();
        public ObservableCollection<Personne> PersonnesDisponibles
        {
            get => personnesDisponibles;
            set { personnesDisponibles = value; OnPropertyChanged(); }
        }

        private Personne? personneSelectionnee;
        public Personne? PersonneSelectionnee
        {
            get => personneSelectionnee;
            set { personneSelectionnee = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
