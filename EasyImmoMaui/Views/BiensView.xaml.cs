using BU.Services;
using Common.Exception;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class BiensView : ContentView
{
    private BiensViewModel biensVM;

    public BiensView()
    {
        InitializeComponent();
        biensVM = new BiensViewModel();
        this.BindingContext = biensVM;
        ChargerBiens();
    }

    private void ChargerBiens()
    {
        biensVM.Biens.Clear();
        foreach (var bien in BienService.GetBiens())
            biensVM.Biens.Add(bien);
    }

    private void Rechercher(object sender, TextChangedEventArgs e)
    {
        biensVM.Biens.Clear();
        foreach (var bien in BienService.RechercherBiens(biensVM.TexteRecherche))
            biensVM.Biens.Add(bien);
    }

    private async void Ajouter(object sender, EventArgs e)
    {
        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new BienFormulairePopup();
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerBiens();
        };
    }

    // Action du bouton Détail 
    private async void VoirDetail(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not Bien bien) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new BienDetailPopup(bien);
        await page.Navigation.PushModalAsync(popup);
    }

    // Action du bouton Documents
    private async void VoirDocuments(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not Bien bien) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new DocumentsPopup(bien.IdBien, bien.NomBien ?? "Bien");
        await page.Navigation.PushModalAsync(popup);
    }

    private async void Modifier(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not Bien bien) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new BienFormulairePopup(bien);
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerBiens();
        };
    }

    private async void Supprimer(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not Bien bien) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        bool confirmer = await page.DisplayAlert(
            "Confirmer",
            $"Voulez-vous vraiment supprimer le bien \"{bien.NomBien}\" ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            BienService.SupprimerBien(bien.IdBien);
            biensVM.Biens.Remove(bien);
            await page.DisplayAlert("Succès", "Bien supprimé avec succès.", "OK");
        }
        catch (EntiteIntrouvableException ex)
        {
            await page.DisplayAlert("Information", ex.Message, "OK");
            ChargerBiens();
        }
        catch (SuppressionImpossibleException ex)
        {
            await page.DisplayAlert("Suppression impossible", ex.Message, "OK");
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
