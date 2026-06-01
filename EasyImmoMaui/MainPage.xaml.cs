using EasyImmoMaui.ViewModels;
using EasyImmoMaui.Views;

namespace EasyImmoMaui
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel mainVM;

        public MainPage()
        {
            InitializeComponent();
            mainVM = new MainViewModel();
            this.BindingContext = mainVM;

           
            Naviguer("TableauDeBord");
        }

        // Méthode appelée quand on clique sur un menu de la sidebar
        private void Naviguer(string destination)
        {
            mainVM.MenuActif = destination;

          
            switch (destination)
            {
                case "TableauDeBord":
                    mainVM.CurrentView = new TableauDeBordView();
                    mainVM.TitrePage = "Tableau de Bord";
                    mainVM.SousTitre = "Bienvenue, voici un aperçu de votre activité";
                    break;

                case "Biens":
                    mainVM.CurrentView = new BiensView();
                    mainVM.TitrePage = "Biens";
                    mainVM.SousTitre = "Operation concernant les biens immobiliers";
                    break;

                case "Clients":
                    mainVM.CurrentView = new ClientsView();
                    mainVM.TitrePage = "Clients";
                    mainVM.SousTitre = "Gestion des clients";
                    break;

                case "Activites":
                    mainVM.CurrentView = new ActivitesView();
                    mainVM.TitrePage = "Activités";
                    mainVM.SousTitre = "Gestion des activités";
                    break;

                case "Operations":
                    mainVM.CurrentView = new ContratsView();
                    mainVM.TitrePage = "Contrats";
                    mainVM.SousTitre = "Signatures de compromis et actes de vente";
                    break;

                case "Parametres":
                    mainVM.CurrentView = new ParametresView();
                    mainVM.TitrePage = "Paramètres";
                    mainVM.SousTitre = "Gestion des employés";
                    break;

                default:
                    mainVM.CurrentView = new TableauDeBordView();
                    mainVM.TitrePage = "Tableau de Bord";
                    mainVM.SousTitre = "Bienvenue, voici un aperçu de votre activité";
                    break;
            }
        }

        private void NaviguerTableauDeBord(object sender, EventArgs e) => Naviguer("TableauDeBord");
        private void NaviguerBiens(object sender, EventArgs e) => Naviguer("Biens");
        private void NaviguerClients(object sender, EventArgs e) => Naviguer("Clients");
        private void NaviguerActivites(object sender, EventArgs e) => Naviguer("Activites");
        private void NaviguerOperations(object sender, EventArgs e) => Naviguer("Operations");
        private void NaviguerParametres(object sender, EventArgs e) => Naviguer("Parametres");
    }
}
