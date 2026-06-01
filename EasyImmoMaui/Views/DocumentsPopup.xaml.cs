using BU.Services;
using DAL.DB;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class DocumentsPopup : ContentPage
{
    private readonly DocumentsViewModel viewModel;
    private readonly int idBien;

    public DocumentsPopup(int idBien, string nomBien)
    {
        InitializeComponent();
        this.idBien = idBien;
        viewModel = new DocumentsViewModel();
        viewModel.TitreBien = nomBien;
        this.BindingContext = viewModel;
        Charger();
    }

    private void Charger()
    {
        var tous = BienService.GetDocuments(idBien);

        viewModel.Photos = new System.Collections.ObjectModel.ObservableCollection<DocumentBien>(
            tous.Where(d => d.DescriptionDoc != null && d.DescriptionDoc.StartsWith("PHOTO:")));

        viewModel.Documents = new System.Collections.ObjectModel.ObservableCollection<DocumentBien>(
            tous.Where(d => d.DescriptionDoc == null || !d.DescriptionDoc.StartsWith("PHOTO:")));
    }


    private async void AjouterPhoto(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Sélectionner une photo"
            });

            if (photo == null) return;

            var dossier = Path.Combine(FileSystem.AppDataDirectory, "biens", idBien.ToString());
            Directory.CreateDirectory(dossier);

            var nomFichier = $"{DateTime.Now:yyyyMMddHHmmss}_{photo.FileName}";
            var destination = Path.Combine(dossier, nomFichier);

            using var sourceStream = await photo.OpenReadAsync();
            using var destStream = File.Create(destination);
            await sourceStream.CopyToAsync(destStream);

            BienService.AjouterDocument(idBien, "PHOTO:" + destination);
            Charger();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible d'ajouter la photo : " + ex.Message, "OK");
        }
    }

   
    private async void AjouterPDF(object sender, EventArgs e)
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Sélectionner un document PDF",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx" } },
                    { DevicePlatform.Android, new[] { "application/pdf", "application/msword" } },
                    { DevicePlatform.iOS, new[] { "public.pdf" } }
                })
            };

            var fichier = await FilePicker.PickAsync(options);
            if (fichier == null) return;

            var dossier = Path.Combine(FileSystem.AppDataDirectory, "biens", idBien.ToString());
            Directory.CreateDirectory(dossier);

            var nomFichier = $"{DateTime.Now:yyyyMMddHHmmss}_{fichier.FileName}";
            var destination = Path.Combine(dossier, nomFichier);

            using var sourceStream = await fichier.OpenReadAsync();
            using var destStream = File.Create(destination);
            await sourceStream.CopyToAsync(destStream);

            BienService.AjouterDocument(idBien, $"PDF:{nomFichier} ({fichier.FileName})");
            Charger();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible d'ajouter le document : " + ex.Message, "OK");
        }
    }

 
    private async void AjouterDescriptionDocument(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(viewModel.NouveauDocument))
        {
            await DisplayAlert("Erreur", "Veuillez entrer une description.", "OK");
            return;
        }

        try
        {
            BienService.AjouterDocument(idBien, viewModel.NouveauDocument);
            viewModel.NouveauDocument = string.Empty;
            Charger();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible d'ajouter le document : " + ex.Message, "OK");
        }
    }

    // Supprime un document ou une photo
    private async void SupprimerDocument(object sender, EventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is not DocumentBien doc) return;

        bool confirmer = await DisplayAlert(
            "Confirmer",
            $"Supprimer ce document ?",
            "Oui",
            "Non");

        if (!confirmer) return;

        try
        {
            BienService.SupprimerDocument(doc.IdDoc);
            Charger();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible de supprimer : " + ex.Message, "OK");
        }
    }

    private async void Fermer(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
