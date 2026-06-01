using BU.Services;
using Common.Exception;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ParticipantsPopup : ContentPage
{
    private readonly ParticipantsViewModel viewModel;
    private readonly int idActivite;

    public ParticipantsPopup(int idActivite)
    {
        InitializeComponent();
        this.idActivite = idActivite;
        viewModel = new ParticipantsViewModel();
        this.BindingContext = viewModel;

        // Charger le titre de l'activité
        var activite = ActiviteServices.GetActiviteById(idActivite);
        if (activite != null)
            viewModel.TitreActivite = activite.LibelleActivite ?? "Activité";

        Charger();
    }

    private void Charger()
    {
        try
        {
            // Participants actuels
            var participants = ActiviteServices.GetParticipants(idActivite);
            viewModel.Participants = new System.Collections.ObjectModel.ObservableCollection<Personne>(participants);

            var toutes = ActiviteServices.GetAllPersonnes();
            var idsParticipants = participants.Select(p => p.IdPersonne).ToList();
            var dispo = toutes.Where(p => !idsParticipants.Contains(p.IdPersonne)).ToList();
            viewModel.PersonnesDisponibles = new System.Collections.ObjectModel.ObservableCollection<Personne>(dispo);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Erreur chargement participants : " + ex.Message);
        }
    }

    private async void AjouterParticipant(object sender, EventArgs e)
    {
        if (viewModel.PersonneSelectionnee == null)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner une personne à ajouter.", "OK");
            return;
        }

        try
        {
            ActiviteServices.AjouterParticipant(idActivite, viewModel.PersonneSelectionnee.IdPersonne);
            Charger();
            viewModel.PersonneSelectionnee = null;
        }
        catch (EntiteIntrouvableException ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
        catch (SuppressionImpossibleException ex)
        {
            await DisplayAlert("Impossible", ex.Message, "OK");
        }
        catch (ErreurBaseDeDonneesException ex)
        {
            await DisplayAlert("Erreur technique", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Erreur inattendue : " + ex.Message, "OK");
        }
    }

    //  supprimer dans la liste des participants
    private async void RetirerParticipant(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not Personne personne) return;

        bool confirmer = await DisplayAlert(
            "Confirmer",
            $"Retirer {personne.PrenomPersonne} {personne.NomPersonne} de cette activité ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            ActiviteServices.RetirerParticipant(idActivite, personne.IdPersonne);
            Charger();
        }
        catch (EntiteIntrouvableException ex)
        {
            await DisplayAlert("Information", ex.Message, "OK");
            Charger();
        }
        catch (ErreurBaseDeDonneesException ex)
        {
            await DisplayAlert("Erreur technique", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Erreur inattendue : " + ex.Message, "OK");
        }
    }

    private async void Fermer(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
