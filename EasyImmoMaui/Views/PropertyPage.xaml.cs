using BU.Services;
using EasyImmoMaui.ViewsModels;
using System.Collections.ObjectModel;

namespace EasyImmoMaui.Views
{
    [QueryProperty(nameof(BienId), "bienId")]
    public partial class PropertyPage : ContentPage
    {
        public int BienId { get; set; }

        public PropertyPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            this.BindingContext = InitPropertyPageViewModel(BienId);
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
