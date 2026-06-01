using BU.Services;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class BienDetailPopup : ContentPage
{
    private readonly BienDetailViewModel viewModel;

    public BienDetailPopup(Bien bien)
    {
        InitializeComponent();
        viewModel = new BienDetailViewModel();
        ChargerDetail(bien);
        this.BindingContext = viewModel;
    }

    private void ChargerDetail(Bien bien)
    {
        // Informations générales
        viewModel.NomBien = bien.NomBien ?? string.Empty;
        viewModel.TypeBien = bien.IdTyNavigation?.DescriptionType ?? "Non défini";
        viewModel.StatutBien = bien.IdStatusNavigation?.LibelleSta ?? "Non défini";
        viewModel.Prix = $"{bien.PrixBien:N0} €";
        viewModel.Superficie = bien.SurfacesBien.HasValue ? $"{bien.SurfacesBien:N0} m²" : "Non renseignée";

        if (bien.IdAdresseNavigation != null)
        {
            var adr = bien.IdAdresseNavigation;
            viewModel.Adresse = $"{adr.Numero} {adr.Rue}, {adr.CodePostal} {adr.Ville}".Trim();
        }

        if (bien.DateChangementStatus.HasValue)
            viewModel.DateChangementStatut = $"Dernier changement de statut le {bien.DateChangementStatus:dd/MM/yyyy}";

        // Propriétaire
        var proprio = BienService.GetProprietaireDuBien(bien.IdBien);
        if (proprio != null)
            viewModel.Proprietaire = proprio.NomComplet;

        // Photos du bien
        var docs = BienService.GetDocuments(bien.IdBien);
        var cheminsPhotos = docs
            .Where(d => d.DescriptionDoc != null && d.DescriptionDoc.StartsWith("PHOTO:"))
            .Select(d => d.DescriptionDoc!.Substring(6))
            .ToList();
        viewModel.Photos = new System.Collections.ObjectModel.ObservableCollection<string>(cheminsPhotos);

        // Historique des activités avec participants
        var activites = BienService.GetActivitesDuBienAvecParticipants(bien.IdBien);
        viewModel.Activites = new System.Collections.ObjectModel.ObservableCollection<ActiviteInfo>(activites);
    }

    private async void Fermer(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
