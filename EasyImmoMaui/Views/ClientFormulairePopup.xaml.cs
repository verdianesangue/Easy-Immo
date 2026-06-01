using BU;
using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ClientFormulairePopup : ContentPage
{
    private readonly ClientFormulaireViewModel viewModel;
    private int? idPersonneEnModification;

    public bool SauvegardeEffectuee => viewModel.SauvegardeEffectuee;

    // Constructeur pour ajouter un nouveau client.
    public ClientFormulairePopup()
    {
        InitializeComponent();
        viewModel = new ClientFormulaireViewModel();
        this.BindingContext = viewModel;
    }

    // Constructeur pour modifier un client existant.
    public ClientFormulairePopup(ClientEntity clientAModifier) : this()
    {
        viewModel.Titre = "Modifier un Client";
        idPersonneEnModification = clientAModifier.IdPersonne;

        viewModel.Nom = clientAModifier.Nom;
        viewModel.Prenom = clientAModifier.Prenom;
        viewModel.Email = clientAModifier.Email;
        viewModel.Telephone = clientAModifier.Telephone;

        viewModel.EstAcheteur = clientAModifier.EstAcheteur;
        viewModel.EstLocataire = clientAModifier.EstLocataire;
        viewModel.EstProprietaire = clientAModifier.EstProprietaire;

        viewModel.BudgetMaxAcheteur = clientAModifier.BudgetMaxAcheteur;
        viewModel.ZoneSouhaiteeAcheteur = clientAModifier.ZoneSouhaiteeAcheteur ?? string.Empty;
        viewModel.MontantMaxLocataire = clientAModifier.MontantMaxLocataire;
        viewModel.TypeProprietaire = clientAModifier.TypeProprietaire;
    }

    // Enregistre les données du formulaire en ajoutant ou modifiant un client selon le contexte
    private async void Enregistrer(object sender, EventArgs e)
    {
        var client = new ClientEntity
        {
            Nom = viewModel.Nom,
            Prenom = viewModel.Prenom,
            Email = viewModel.Email,
            Telephone = viewModel.Telephone,
            EstAcheteur = viewModel.EstAcheteur,
            EstLocataire = viewModel.EstLocataire,
            EstProprietaire = viewModel.EstProprietaire,
            BudgetMaxAcheteur = viewModel.EstAcheteur ? viewModel.BudgetMaxAcheteur : null,
            ZoneSouhaiteeAcheteur = viewModel.EstAcheteur ? viewModel.ZoneSouhaiteeAcheteur : null,
            MontantMaxLocataire = viewModel.EstLocataire ? viewModel.MontantMaxLocataire : null,
            TypeProprietaire = viewModel.TypeProprietaire
        };

        // Vérification de la validité des données
        var validation = PersonneServices.ValiderClient(client);
        if (!validation.EstValide)
        {
            await DisplayAlert("Erreur", validation.ToMessage(), "OK");
            return;
        }

        try
        {
            if (idPersonneEnModification.HasValue)
            {
                client.IdPersonne = idPersonneEnModification.Value;
                PersonneServices.ModifierClient(client);
            }
            else
            {
                PersonneServices.AjouterClient(client);
            }

            viewModel.SauvegardeEffectuee = true;
            await Navigation.PopModalAsync();
        }
        catch (EntiteIntrouvableException ex)
        {
            await DisplayAlert("Information", ex.Message, "OK");
            viewModel.SauvegardeEffectuee = true;
            await Navigation.PopModalAsync();
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

    private async void Annuler(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
