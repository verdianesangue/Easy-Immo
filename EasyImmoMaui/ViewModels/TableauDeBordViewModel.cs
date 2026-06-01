using BU.Services;
using Microcharts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    // Affiche les statistiques clés (nombre de biens, clients, visites, contrats), les tendances (variations par rapport au mois précédent)
    public class TableauDeBordViewModel : INotifyPropertyChanged
    {
      
        private int nombreBiens;
        public int NombreBiens
        {
            get => nombreBiens;
            set { nombreBiens = value; NotifyPropertyChanged(); }
        }

        private int nombreClients;
        public int NombreClients
        {
            get => nombreClients;
            set { nombreClients = value; NotifyPropertyChanged(); }
        }

        private int nombreVisites;
        public int NombreVisites
        {
            get => nombreVisites;
            set { nombreVisites = value; NotifyPropertyChanged(); }
        }

        private int nombreContrats;
        public int NombreContrats
        {
            get => nombreContrats;
            set { nombreContrats = value; NotifyPropertyChanged(); }
        }

        // Tendances (variations)
        private string tendanceBiens = string.Empty;
        public string TendanceBiens
        {
            get => tendanceBiens;
            set { tendanceBiens = value; NotifyPropertyChanged(); }
        }

        private string tendanceClients = string.Empty;
        public string TendanceClients
        {
            get => tendanceClients;
            set { tendanceClients = value; NotifyPropertyChanged(); }
        }

        private string tendanceVisites = string.Empty;
        public string TendanceVisites
        {
            get => tendanceVisites;
            set { tendanceVisites = value; NotifyPropertyChanged(); }
        }

        private string tendanceContrats = string.Empty;
        public string TendanceContrats
        {
            get => tendanceContrats;
            set { tendanceContrats = value; NotifyPropertyChanged(); }
        }

        // Les graphiques
        private Chart? graphiqueVentes;
        public Chart? GraphiqueVentes
        {
            get => graphiqueVentes;
            set { graphiqueVentes = value; NotifyPropertyChanged(); }
        }

        private Chart? graphiqueRepartition;
        public Chart? GraphiqueRepartition
        {
            get => graphiqueRepartition;
            set { graphiqueRepartition = value; NotifyPropertyChanged(); }
        }

        private int nombreProprietaires;
        public int NombreProprietaires
        {
            get => nombreProprietaires;
            set { nombreProprietaires = value; NotifyPropertyChanged(); }
        }

        private string tendanceProprietaires = string.Empty;
        public string TendanceProprietaires
        {
            get => tendanceProprietaires;
            set { tendanceProprietaires = value; NotifyPropertyChanged(); }
        }

        private int nombreLocataires;
        public int NombreLocataires
        {
            get => nombreLocataires;
            set { nombreLocataires = value; NotifyPropertyChanged(); }
        }

        private int nombreAcheteurs;
        public int NombreAcheteurs
        {
            get => nombreAcheteurs;
            set { nombreAcheteurs = value; NotifyPropertyChanged(); }
        }

        private int nombreBiensVendus;
        public int NombreBiensVendus
        {
            get => nombreBiensVendus;
            set { nombreBiensVendus = value; NotifyPropertyChanged(); }
        }

        private int nombreBiensLoues;
        public int NombreBiensLoues
        {
            get => nombreBiensLoues;
            set { nombreBiensLoues = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<DAL.DB.Bien> biensVendus = new();
        public ObservableCollection<DAL.DB.Bien> BiensVendus
        {
            get => biensVendus;
            set { biensVendus = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<DAL.DB.Bien> biensLoues = new();
        public ObservableCollection<DAL.DB.Bien> BiensLoues
        {
            get => biensLoues;
            set { biensLoues = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<ProprietaireInfo> listeProprietaires = new();
        public ObservableCollection<ProprietaireInfo> ListeProprietaires
        {
            get => listeProprietaires;
            set { listeProprietaires = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<ClientEntity> listeLocataires = new();
        public ObservableCollection<ClientEntity> ListeLocataires
        {
            get => listeLocataires;
            set { listeLocataires = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<ClientEntity> listeAcheteurs = new();
        public ObservableCollection<ClientEntity> ListeAcheteurs
        {
            get => listeAcheteurs;
            set { listeAcheteurs = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
