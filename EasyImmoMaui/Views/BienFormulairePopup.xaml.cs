using BU.Services;
using Common.Exception;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class BienFormulairePopup : ContentPage
{
    private readonly BienFormulaireViewModel viewModel;
    private int? idBienEnModification;

    public bool SauvegardeEffectuee => viewModel.SauvegardeEffectuee;

    // Constructeur pour ajouter un nouveau bien
    public BienFormulairePopup()
    {
        InitializeComponent();
        viewModel = new BienFormulaireViewModel();
        viewModel.TypesDisponibles = BienService.GetAllTypes();
        viewModel.StatutsDisponibles = BienService.GetAllStatuts();
        viewModel.ProprietairesDisponibles = BienService.GetAllProprietaires();
        this.BindingContext = viewModel;
    }

    // Constructeur pour modifier un bien existant
    public BienFormulairePopup(Bien bienAModifier) : this()
    {
        viewModel.Titre = "Modifier un Bien";
        idBienEnModification = bienAModifier.IdBien;

        viewModel.Nom = bienAModifier.NomBien ?? string.Empty;
        viewModel.Prix = bienAModifier.PrixBien;
        viewModel.Superficie = bienAModifier.SurfacesBien;
        viewModel.Description = bienAModifier.DescriptionBien ?? string.Empty;
        viewModel.DatePublication = bienAModifier.DatePublicationBien ?? DateTime.Today;

        if (bienAModifier.IdTy.HasValue)
            viewModel.TypeSelectionne = viewModel.TypesDisponibles.FirstOrDefault(t => t.IdTy == bienAModifier.IdTy);

        if (bienAModifier.IdStatus.HasValue)
            viewModel.StatutSelectionne = viewModel.StatutsDisponibles.FirstOrDefault(s => s.IdSta == bienAModifier.IdStatus);

        if (bienAModifier.IdAdresseNavigation != null)
        {
            viewModel.AdresseNumero = bienAModifier.IdAdresseNavigation.Numero ?? string.Empty;
            viewModel.AdresseRue = bienAModifier.IdAdresseNavigation.Rue ?? string.Empty;
            viewModel.AdresseVille = bienAModifier.IdAdresseNavigation.Ville ?? string.Empty;
            viewModel.AdresseCodePostal = bienAModifier.IdAdresseNavigation.CodePostal ?? string.Empty;
            viewModel.AdressePays = bienAModifier.IdAdresseNavigation.Pays ?? string.Empty;
        }

        // Charger le propriétaire actuel du bien s'il en a un
        var proprietaireActuel = BienService.GetProprietaireDuBien(bienAModifier.IdBien);
        if (proprietaireActuel != null)
            viewModel.ProprietaireSelectionne = viewModel.ProprietairesDisponibles
                .FirstOrDefault(p => p.IdPro == proprietaireActuel.IdPro);
    }

    // Action du bouton photo dans le formulaire
    private async void AjouterPhoto(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Sélectionner une photo"
            });
            if (photo != null)
                viewModel.PhotosEnAttente.Add(photo.FullPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible d'accéder aux photos : " + ex.Message, "OK");
        }
    }

    private async void Enregistrer(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(viewModel.Nom))
        {
            await DisplayAlert("Erreur", "Le nom du bien est obligatoire.", "OK");
            return;
        }
        if (viewModel.Prix <= 0)
        {
            await DisplayAlert("Erreur", "Le prix doit être supérieur à 0.", "OK");
            return;
        }
        if (viewModel.TypeSelectionne == null)
        {
            await DisplayAlert("Erreur", "Veuillez sélectionner un type.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(viewModel.AdresseVille))
        {
            await DisplayAlert("Erreur", "La ville est obligatoire.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(viewModel.AdressePays))
        {
            await DisplayAlert("Erreur", "Le pays est obligatoire.", "OK");
            return;
        }

        try
        {
            // Créer ou récupérer l'adresse
            int? idAdresse = null;

            if (idBienEnModification.HasValue)
            {
                var bienExistant = BienService.GetBienById(idBienEnModification.Value);
                if (bienExistant?.IdAdresse.HasValue == true)
                {
                    var adresseExistante = new Adresse
                    {
                        IdAdresse = bienExistant.IdAdresse.Value,
                        Numero = viewModel.AdresseNumero,
                        Rue = viewModel.AdresseRue,
                        Ville = viewModel.AdresseVille,
                        CodePostal = viewModel.AdresseCodePostal,
                        Pays = viewModel.AdressePays
                    };
                    BienService.ModifierAdresse(adresseExistante);
                    idAdresse = bienExistant.IdAdresse.Value;
                }
                else
                {
                    var nouvelleAdresse = new Adresse
                    {
                        Numero = viewModel.AdresseNumero,
                        Rue = viewModel.AdresseRue,
                        Ville = viewModel.AdresseVille,
                        CodePostal = viewModel.AdresseCodePostal,
                        Pays = viewModel.AdressePays
                    };
                    idAdresse = BienService.AjouterAdresse(nouvelleAdresse);
                }
            }
            else
            {
                var nouvelleAdresse = new Adresse
                {
                    Numero = viewModel.AdresseNumero,
                    Rue = viewModel.AdresseRue,
                    Ville = viewModel.AdresseVille,
                    CodePostal = viewModel.AdresseCodePostal,
                    Pays = viewModel.AdressePays
                };
                idAdresse = BienService.AjouterAdresse(nouvelleAdresse);
            }

            // Créer ou modifier le bien
            var bien = new Bien
            {
                NomBien = viewModel.Nom,
                PrixBien = viewModel.Prix,
                SurfacesBien = viewModel.Superficie,
                DescriptionBien = viewModel.Description,
                DatePublicationBien = viewModel.DatePublication,
                IdTy = viewModel.TypeSelectionne?.IdTy,
                IdAdresse = idAdresse,
                IdStatus = viewModel.StatutSelectionne?.IdSta
            };

            if (idBienEnModification.HasValue)
            {
                bien.IdBien = idBienEnModification.Value;
                BienService.ModifierBien(bien);
            }
            else
            {
                BienService.AjouterBien(bien);
            }

            // Lier le propriétaire sélectionné au bien s'il y en a un
            if (viewModel.ProprietaireSelectionne != null)
                BienService.AssocierProprietaire(bien.IdBien, viewModel.ProprietaireSelectionne.IdPro);

            // Sauvegarder les photos sélectionnées dans le formulaire
            foreach (var cheminSource in viewModel.PhotosEnAttente)
            {
                var dossier = Path.Combine(FileSystem.AppDataDirectory, "biens", bien.IdBien.ToString());
                Directory.CreateDirectory(dossier);
                var nomFichier = $"{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(cheminSource)}";
                var destination = Path.Combine(dossier, nomFichier);
                File.Copy(cheminSource, destination, overwrite: true);
                BienService.AjouterDocument(bien.IdBien, "PHOTO:" + destination);
            }

            viewModel.SauvegardeEffectuee = true;
            await Navigation.PopModalAsync();
        }
        catch (SuppressionImpossibleException ex)
        {
            await DisplayAlert("Transition impossible", ex.Message, "OK");
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
