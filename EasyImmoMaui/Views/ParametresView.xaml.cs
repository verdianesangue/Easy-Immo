using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ParametresView : ContentView
{
    private ParametresViewModel parametresVM;

    public ParametresView()
    {
        InitializeComponent();
        parametresVM = new ParametresViewModel();
        this.BindingContext = parametresVM;
        ChargerEmployes();
    }

    private void ChargerEmployes()
    {
        parametresVM.Employes.Clear();
        foreach (var e in PersonneServices.GetEmployes())
            parametresVM.Employes.Add(e);
    }

    private void Rechercher(object sender, TextChangedEventArgs e)
    {
        parametresVM.Employes.Clear();
        foreach (var emp in PersonneServices.RechercherEmployes(parametresVM.TexteRecherche))
            parametresVM.Employes.Add(emp);
    }

    private async void Ajouter(object sender, EventArgs e)
    {
        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new EmployeFormulairePopup();
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerEmployes();
        };
    }

    private async void Modifier(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not EmployeEntity employe) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new EmployeFormulairePopup(employe);
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerEmployes();
        };
    }

    private async void Supprimer(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not EmployeEntity employe) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        bool confirmer = await page.DisplayAlert(
            "Confirmer",
            $"Voulez-vous vraiment supprimer l'employé \"{employe.NomComplet}\" ?",
            "Oui", "Non");

        if (!confirmer) return;

        try
        {
            PersonneServices.SupprimerEmploye(employe.IdEmp);
            parametresVM.Employes.Remove(employe);
            await page.DisplayAlert("Succès", "Employé supprimé avec succès.", "OK");
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
