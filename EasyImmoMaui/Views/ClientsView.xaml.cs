using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ClientsView : ContentView
{
    private ClientsViewModel clientsVM;

    public ClientsView()
    {
        InitializeComponent();
        clientsVM = new ClientsViewModel();
        this.BindingContext = clientsVM;
        ChargerClients();
    }

    // Charge tous les clients depuis la base de données
    private void ChargerClients()
    {
        clientsVM.Clients.Clear();
        foreach (var client in PersonneServices.GetClients())
            clientsVM.Clients.Add(client);
    }

    // Filtre les clients selon le texte de recherche
    private void Rechercher(object sender, TextChangedEventArgs e)
    {
        clientsVM.Clients.Clear();
        foreach (var client in PersonneServices.RechercherClients(clientsVM.TexteRecherche))
            clientsVM.Clients.Add(client);
    }

  
    private async void Ajouter(object sender, EventArgs e)
    {
        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ClientFormulairePopup();
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerClients();
        };
    }

    private async void Modifier(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ClientEntity client) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        var popup = new ClientFormulairePopup(client);
        await page.Navigation.PushModalAsync(popup);

        popup.Disappearing += (s, args) =>
        {
            if (popup.SauvegardeEffectuee)
                ChargerClients();
        };
    }

  
    private async void Supprimer(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not ClientEntity client) return;

        var page = Application.Current?.MainPage;
        if (page == null) return;

        bool confirmer = await page.DisplayAlert(
            "Confirmer",
            $"Voulez-vous vraiment supprimer le client \"{client.NomComplet}\" ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            PersonneServices.SupprimerClient(client.IdPersonne);
            clientsVM.Clients.Remove(client);
            await page.DisplayAlert("Succès", "Client supprimé avec succès.", "OK");
        }
        catch (EntiteIntrouvableException ex)
        {
            await page.DisplayAlert("Information", ex.Message, "OK");
            ChargerClients();
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
