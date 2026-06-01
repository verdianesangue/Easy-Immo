using BU;
using BU.Services;
using Common.Exception;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class ActiviteFormulairePopup : ContentPage
{
    private readonly ActiviteFormulaireViewModel viewModel;
    private int? idActiviteEnModification;

    public bool SauvegardeEffectuee => viewModel.SauvegardeEffectuee;

    // Liste des types avec noms lisibles
    private static readonly List<TypeActiviteAffichage> TypesLisibles = new()
    {
        new() { Type = TypeActiviteEnum.Visite,             Nom = "Visite" },
        new() { Type = TypeActiviteEnum.AppelTelephonique,  Nom = "Appel téléphonique" },
        new() { Type = TypeActiviteEnum.EnvoiEmail,         Nom = "Envoi email" },
        new() { Type = TypeActiviteEnum.SignatureContrat,   Nom = "Signature contrat" },
        new() { Type = TypeActiviteEnum.AnalyseDossier,     Nom = "Analyse dossier" },
        new() { Type = TypeActiviteEnum.RendezVousAgence,   Nom = "Rendez-vous agence" },
        new() { Type = TypeActiviteEnum.PublicationAnnonce, Nom = "Publication annonce" },
        new() { Type = TypeActiviteEnum.MiseAJourDossier,   Nom = "Mise à jour dossier" },
        new() { Type = TypeActiviteEnum.RelanceClient,      Nom = "Relance client" },
        new() { Type = TypeActiviteEnum.EvaluationDuBien,   Nom = "Évaluation du bien" },
        new() { Type = TypeActiviteEnum.DemandeInfos,       Nom = "Demande infos" },
        new() { Type = TypeActiviteEnum.SignatureCompromis, Nom = "Signature compromis" }
    };

    public ActiviteFormulairePopup(int? idActivite = null)
    {
        InitializeComponent();
        viewModel = new ActiviteFormulaireViewModel();
        idActiviteEnModification = idActivite;

        // Charger les types lisibles
        viewModel.TypesDisponibles = new System.Collections.ObjectModel.ObservableCollection<TypeActiviteAffichage>(TypesLisibles);
        viewModel.TypeSelectionne = viewModel.TypesDisponibles.First();

        // Charger la liste des biens
        ChargerBiens();

        this.BindingContext = viewModel;

        // Si on est en modification, charger les données de l'activité
        if (idActivite.HasValue)
        {
            viewModel.TitrePopup = "Modifier l'activité";
            ChargerActivite(idActivite.Value);
        }
    }

    private void ChargerBiens()
    {
        try
        {
            var biens = ActiviteServices.GetAllBiens();
            viewModel.BiensDisponibles.Clear();
            foreach (var b in biens)
                viewModel.BiensDisponibles.Add(b);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Erreur chargement biens : " + ex.Message);
        }
    }

    private void ChargerActivite(int idActivite)
    {
        try
        {
            var activite = ActiviteServices.GetActiviteById(idActivite);
            if (activite == null) return;

            viewModel.Libelle = activite.LibelleActivite ?? string.Empty;

            if (activite.DateActivite.HasValue)
            {
                viewModel.DateActivite = activite.DateActivite.Value.Date;
                viewModel.HeureActivite = activite.DateActivite.Value.TimeOfDay;
            }

            if (activite.DureeActivite.HasValue)
            {
                var d = activite.DureeActivite.Value;
                viewModel.DureeActivite = new TimeSpan(d.Hour, d.Minute, 0);
            }

            // Retrouver le TypeActiviteAffichage correspondant
            viewModel.TypeSelectionne = viewModel.TypesDisponibles
                .FirstOrDefault(t => (int)t.Type == activite.IdType)
                ?? viewModel.TypesDisponibles.First();

            viewModel.BienSelectionne = viewModel.BiensDisponibles
                .FirstOrDefault(b => b.IdBien == activite.IdBien);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Erreur chargement activité : " + ex.Message);
        }
    }

    private async void Enregistrer(object sender, EventArgs e)
    {
        if (viewModel.TypeSelectionne == null)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner un type d'activité.", "OK");
            return;
        }

        // Construire l'objet Activite
        var activite = new Activite
        {
            LibelleActivite = viewModel.Libelle,
            DateActivite    = viewModel.DateActivite.Date + viewModel.HeureActivite,
            DureeActivite   = new TimeOnly(viewModel.DureeActivite.Hours, viewModel.DureeActivite.Minutes),
            IdBien          = viewModel.BienSelectionne?.IdBien ?? 0,
            IdType          = (int)viewModel.TypeSelectionne.Type
        };

        // Validation
        var validation = ActiviteServices.ValiderActivite(activite);
        if (!validation.EstValide)
        {
            await DisplayAlert("Erreur", validation.ToMessage(), "OK");
            return;
        }

        try
        {
            if (idActiviteEnModification.HasValue)
            {
                activite.IdActivite = idActiviteEnModification.Value;
                ActiviteServices.ModifierActivite(activite);
            }
            else
            {
                ActiviteServices.AjouterActivite(activite);
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
