using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class EmployeFormulaireViewModel : INotifyPropertyChanged
    {
        private string titre = "Ajouter un employé";
        public string Titre
        {
            get => titre;
            set { titre = value; OnPropertyChanged(); }
        }

        private string nom = string.Empty;
        public string Nom
        {
            get => nom;
            set { nom = value; OnPropertyChanged(); }
        }

        private string prenom = string.Empty;
        public string Prenom
        {
            get => prenom;
            set { prenom = value; OnPropertyChanged(); }
        }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string telephone = string.Empty;
        public string Telephone
        {
            get => telephone;
            set { telephone = value; OnPropertyChanged(); }
        }

        private string role = string.Empty;
        public string Role
        {
            get => role;
            set { role = value; OnPropertyChanged(); }
        }

        public bool SauvegardeEffectuee { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
