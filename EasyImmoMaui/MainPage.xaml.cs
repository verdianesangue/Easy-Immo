using BU.Services;
using EasyImmoMaui.ViewsModels;
using System.Collections.ObjectModel;

namespace EasyImmoMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var premierBien = BienService.GetBiens().FirstOrDefault();
            if (premierBien != null)
                this.BindingContext = InitPropertyPageViewModel(premierBien.IdBien);
        }

        PropertyPageViewModel InitPropertyPageViewModel(int propertyID)
        {
            var vm = new PropertyPageViewModel();

            var bien = BienService.GetBienDetail(propertyID);
            if (bien != null)
            {
                vm.IdBien = bien.IdBien;
                vm.TypeBien = bien.IdTyNavigation?.DescriptionType ?? "Inconnu";
                vm.Statut = bien.HistoriqueStatusBien?.Id1?.LibelleSta ?? "Inconnu";
                vm.Description = bien.DescriptionBien ?? string.Empty;
            }

            var activites = ActiviteServices.GetActivitesByBien(propertyID);
            vm.Activites = new ObservableCollection<DAL.DB.Activite>(activites);

            return vm;
        }
    }
}
