using BU;
using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;
using SuppressionImpossibleException = Common.Exception.SuppressionImpossibleException;

namespace EasyImmoMaui.Views;

public partial class ContratFormulairePopup : ContentPage
{
    private readonly ContratFormulaireViewModel viewModel;
    private int? idContratEnModification;

    public bool SauvegardeEffectuee => viewModel.SauvegardeEffectuee;

    public ContratFormulairePopup()
    {
        InitializeComponent();
        viewModel = new ContratFormulaireViewModel();

        viewModel.BiensDisponibles         = BienService.GetBiensDisponibles();
        viewModel.AgentsDisponibles        = ContratService.GetAgents();
        viewModel.AcheteursDisponibles     = ContratService.GetAcheteurs();
        viewModel.LocatairesDisponibles    = ContratService.GetLocataires();
        viewModel.ProprietairesDisponibles = ContratService.GetProprietaires();

        this.BindingContext = viewModel;

        MettreAJourPickerParticipant();

        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ContratFormulaireViewModel.TypeContrat))
                MettreAJourPickerParticipant();
        };
    }

    // Constructeur modification
    public ContratFormulairePopup(ContratModification contrat) : this()
    {
        idContratEnModification = contrat.IdContrat;
        viewModel.Titre = "Modifier un contrat";

        viewModel.TypeContrat  = Enum.TryParse<TypeContrat>(contrat.TypeContrat, out var tc) ? tc : TypeContrat.Vente;
        viewModel.Description  = contrat.Description;
        viewModel.DateOperation = contrat.DateOperation ?? DateTime.Today;

        viewModel.BienSelectionne        = viewModel.BiensDisponibles.FirstOrDefault(b => b.IdBien == contrat.IdBien);
        viewModel.AgentSelectionne       = viewModel.AgentsDisponibles.FirstOrDefault(a => a.IdEmp == contrat.IdEmp);
        viewModel.ProprietaireSelectionne = viewModel.ProprietairesDisponibles.FirstOrDefault(p => p.IdPersonne == contrat.IdPersonneProprietaire);

        MettreAJourPickerParticipant();
        viewModel.ParticipantSelectionne = ((List<ProprietaireInfo>)PickerParticipant.ItemsSource)
            .FirstOrDefault(p => p.IdPersonne == contrat.IdPersonneParticipant);
    }

    private void MettreAJourPickerParticipant()
    {
        viewModel.ParticipantSelectionne = null;
        PickerParticipant.ItemsSource = viewModel.TypeContrat == TypeContrat.Vente
            ? viewModel.AcheteursDisponibles
            : viewModel.LocatairesDisponibles;
    }

    private async void Enregistrer(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(viewModel.Description))
        {
            await DisplayAlert("Erreur", "La description est obligatoire.", "OK");
            return;
        }
        if (viewModel.BienSelectionne == null)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner un bien.", "OK");
            return;
        }

        try
        {
            if (idContratEnModification.HasValue)
            {
                ContratService.ModifierContrat(
                    idContrat: idContratEnModification.Value,
                    description: viewModel.Description,
                    date: viewModel.DateOperation,
                    typeContrat: viewModel.TypeContrat,
                    idEmp: viewModel.AgentSelectionne?.IdEmp,
                    idPersonneParticipant: viewModel.ParticipantSelectionne?.IdPersonne,
                    idPersonneProprietaire: viewModel.ProprietaireSelectionne?.IdPersonne);
            }
            else
            {
                ContratService.AjouterContrat(
                    description: viewModel.Description,
                    date: viewModel.DateOperation,
                    idBien: viewModel.BienSelectionne.IdBien,
                    typeContrat: viewModel.TypeContrat,
                    idEmp: viewModel.AgentSelectionne?.IdEmp,
                    idPersonneAcheteurOuLocataire: viewModel.ParticipantSelectionne?.IdPersonne,
                    idPersonneProprietaire: viewModel.ProprietaireSelectionne?.IdPersonne);
            }

            viewModel.SauvegardeEffectuee = true;
            await Navigation.PopModalAsync();
        }
        catch (SuppressionImpossibleException ex)
        {
            await DisplayAlert("Impossible", ex.Message, "OK");
        }
        catch (EntiteIntrouvableException ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
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
