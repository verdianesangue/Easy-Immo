using BU.Services;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ContratDetailPopup : ContentPage
{
    public ContratDetailPopup(int idContrat)
    {
        InitializeComponent();

        var detail = ContratService.GetContratDetail(idContrat);
        if (detail == null)
        {
            Navigation.PopModalAsync();
            return;
        }

        var vm = new ContratDetailViewModel
        {
            Description = detail.Description,
            TypeContrat = detail.TypeContrat,
            DateContrat = detail.DateContrat?.ToString("dd/MM/yyyy") ?? "—",
            NomBien = detail.NomBien,
            Adresse = detail.Adresse,
            Prix = detail.PrixBien.HasValue ? $"{detail.PrixBien:N0} €" : "—",
            NomAgent = detail.NomAgent,
            LabelParticipant = detail.LabelParticipant,
            NomParticipantPrincipal = detail.NomParticipantPrincipal,
            NomProprietaire = detail.NomProprietaire
        };

        this.BindingContext = vm;
    }

    private async void Fermer(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
