using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ContratsView : ContentView
{
    private ContratsViewModel contratsVM;

    public ContratsView()
    {
        InitializeComponent();
        contratsVM = new ContratsViewModel();
        this.BindingContext = contratsVM;
        ChargerContrats();
    }

    private void ChargerContrats()
    {
        try
        {
            var liste = ContratService.GetContrats();
            contratsVM.Contrats = new System.Collections.ObjectModel.ObservableCollection<ContratAffichage>(liste);
        }
        catch (Exception ex)
        {
            var page = Application.Current?.MainPage;
            page?.DisplayAlert("Erreur", "Impossible de charger les contrats : " + ex.Message, "OK");
        }
    }

    private async void VoirDetail(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ContratAffichage contrat) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ContratDetailPopup(contrat.IdContrat);
        await page.Navigation.PushModalAsync(popup);
    }

    private async void Ajouter(object sender, EventArgs e)
    {
        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ContratFormulairePopup();
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerContrats();
        };
    }

    private async void Modifier(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ContratAffichage contrat) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var donnees = ContratService.GetContratPourModification(contrat.IdContrat);
        if (donnees == null)
        {
            await page.DisplayAlert("Erreur", "Contrat introuvable.", "OK");
            return;
        }

        var popup = new ContratFormulairePopup(donnees);
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerContrats();
        };
    }

    private async void Supprimer(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ContratAffichage contrat) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        bool confirmer = await page.DisplayAlert(
            "Confirmer",
            $"Voulez-vous vraiment supprimer le contrat \"{contrat.Description}\" ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            ContratService.SupprimerContrat(contrat.IdContrat);
            contratsVM.Contrats.Remove(contrat);
            await page.DisplayAlert("Succès", "Contrat supprimé avec succès.", "OK");
        }
        catch (EntiteIntrouvableException ex)
        {
            await page.DisplayAlert("Information", ex.Message, "OK");
            ChargerContrats();
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
