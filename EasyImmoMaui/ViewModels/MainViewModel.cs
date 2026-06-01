using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    // ViewModel principal qui gère la navigation entre les pages et le menu actif de la sidebar
    public partial class MainViewModel : INotifyPropertyChanged
    {
        // La vue actuellement affichée au centre
        private View currentView = new ContentView();
        public View CurrentView
        {
            get => currentView;
            set { currentView = value; NotifyPropertyChanged(); }
        }

        private string menuActif = "TableauDeBord";
        public string MenuActif
        {
            get => menuActif;
            set { menuActif = value; NotifyPropertyChanged(); }
        }

        // Le titre affiché en haut de la zone centrale
        private string titrePage = "Tableau de Bord";
        public string TitrePage
        {
            get => titrePage;
            set { titrePage = value; NotifyPropertyChanged(); }
        }

        // Le sous-titre affiché sous le titre
        private string sousTitre = "Bienvenue, voici un aperçu de votre activité";
        public string SousTitre
        {
            get => sousTitre;
            set { sousTitre = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
