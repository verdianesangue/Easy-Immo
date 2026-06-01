using BU.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        // La liste des clients affichée dans le tableau
        private ObservableCollection<ClientEntity> clients = new();
        public ObservableCollection<ClientEntity> Clients
        {
            get => clients;
            set { clients = value; NotifyPropertyChanged(); }
        }

        // Le texte tapé dans la barre de recherche
        private string texteRecherche = string.Empty;
        public string TexteRecherche
        {
            get => texteRecherche;
            set { texteRecherche = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
