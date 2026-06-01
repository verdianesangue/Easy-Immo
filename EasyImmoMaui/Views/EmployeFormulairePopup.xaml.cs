using BU.Services;
using Common.Exception;
using EasyImmoMaui.ViewModels;

namespace EasyImmoMaui.Views;

public partial class EmployeFormulairePopup : ContentPage
{
    private readonly EmployeFormulaireViewModel viewModel;
    private int? idEmpEnModification;

    public bool SauvegardeEffectuee => viewModel.SauvegardeEffectuee;

    // Constructeur ajout
    public EmployeFormulairePopup()
    {
        InitializeComponent();
        viewModel = new EmployeFormulaireViewModel();
        this.BindingContext = viewModel;
    }

    // Constructeur modification
    public EmployeFormulairePopup(EmployeEntity employe) : this()
    {
        viewModel.Titre = "Modifier un employé";
        idEmpEnModification = employe.IdEmp;

        viewModel.Nom       = employe.Nom;
        viewModel.Prenom    = employe.Prenom;
        viewModel.Email     = employe.Email;
        viewModel.Telephone = employe.Telephone;
        viewModel.Role      = employe.Role;
    }

    private async void Enregistrer(object sender, EventArgs e)
    {
        var employe = new EmployeEntity
        {
            Nom       = viewModel.Nom,
            Prenom    = viewModel.Prenom,
            Email     = viewModel.Email,
            Telephone = viewModel.Telephone,
            Role      = viewModel.Role
        };

        var validation = PersonneServices.ValiderEmploye(employe);
        if (!validation.EstValide)
        {
            await DisplayAlert("Erreur", validation.ToMessage(), "OK");
            return;
        }

        try
        {
            if (idEmpEnModification.HasValue)
            {
                employe.IdEmp = idEmpEnModification.Value;
                PersonneServices.ModifierEmploye(employe);
            }
            else
            {
                PersonneServices.AjouterEmploye(employe);
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
            var msgs = new System.Text.StringBuilder();
            var cur = (System.Exception)ex;
            while (cur != null) { msgs.AppendLine(cur.Message); cur = cur.InnerException; }
            await DisplayAlert("Erreur technique", msgs.ToString(), "OK");
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
