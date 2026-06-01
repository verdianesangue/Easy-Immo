using BU.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ContratsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ContratAffichage> contrats = new();
        public ObservableCollection<ContratAffichage> Contrats
        {
            get => contrats;
            set { contrats = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
