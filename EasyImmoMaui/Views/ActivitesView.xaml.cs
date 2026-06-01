using BU;
using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ActivitesView : ContentView
{
    private ActivitesViewModel activitesVM;

    public ActivitesView()
    {
        InitializeComponent();
        activitesVM = new ActivitesViewModel();
        this.BindingContext = activitesVM;
        ChargerActivites();
    }

    // Charge toutes les activités depuis la base de données
    private void ChargerActivites()
    {
        try
        {
            var liste = string.IsNullOrWhiteSpace(activitesVM.Recherche)
                ? ActiviteServices.GetActivites()
                : ActiviteServices.RechercherActivites(activitesVM.Recherche);

            var affichages = liste.Select(a => new ActiviteAffichage
            {
                Activite = a,
                NomBien = a.IdBienNavigation?.NomBien ?? "Bien inconnu",
                NomType = a.IdTypeNavigation?.DescriptionType ?? "Inconnu"
            }).ToList();

            activitesVM.Activites = new System.Collections.ObjectModel.ObservableCollection<ActiviteAffichage>(affichages);
        }
        catch (Exception ex)
        {
            var page = Application.Current?.MainPage;
            page?.DisplayAlert("Erreur", "Impossible de charger les activités : " + ex.Message, "OK");
        }
    }

    // Filtre les activités selon le texte de recherche
    private void Rechercher(object sender, TextChangedEventArgs e)
    {
        ChargerActivites();
    }

    // Action du bouton
    private async void Ajouter(object sender, EventArgs e)
    {
        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ActiviteFormulairePopup();
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerActivites();
        };
    }

    // Action du bouton 
    private async void Modifier(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ActiviteAffichage activite) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ActiviteFormulairePopup(activite.IdActivite);
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerActivites();
        };
    }

    // Action du bouton 
    private async void VoirParticipants(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ActiviteAffichage activite) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ParticipantsPopup(activite.IdActivite);
        await page.Navigation.PushModalAsync(popup);
    }

    // Action du bouton 
    private async void Supprimer(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ActiviteAffichage activite) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        bool confirmer = await page.DisplayAlert(
            "Confirmer",
            $"Voulez-vous vraiment supprimer l'activité \"{activite.LibelleActivite}\" ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            ActiviteServices.SupprimerActivite(activite.IdActivite);
            activitesVM.Activites.Remove(activite);
            await page.DisplayAlert("Succès", "Activité supprimée avec succès.", "OK");
        }
        catch (EntiteIntrouvableException ex)
        {
            await page.DisplayAlert("Information", ex.Message, "OK");
            ChargerActivites();
        }
        catch (ErreurBaseDeDonneesException ex)
        {
            await page.DisplayAlert("Erreur technique", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await page.DisplayAlert("Erreur", "Erreur inattendue : " + ex.Message, "OK");
        }
    }
}
