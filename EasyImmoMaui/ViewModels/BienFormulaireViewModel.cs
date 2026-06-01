using BU.Services;
using DAL.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class BienFormulaireViewModel : INotifyPropertyChanged
    {
        private string titre = "Ajouter un Bien";
        public string Titre
        {
            get => titre;
            set { titre = value; NotifyPropertyChanged(); }
        }

        private string nom = string.Empty;
        public string Nom
        {
            get => nom;
            set { nom = value; NotifyPropertyChanged(); }
        }

        private decimal? superficie;
        public decimal? Superficie
        {
            get => superficie;
            set { superficie = value; NotifyPropertyChanged(); }
        }

        private decimal prix;
        public decimal Prix
        {
            get => prix;
            set { prix = value; NotifyPropertyChanged(); }
        }

        private string description = string.Empty;
        public string Description
        {
            get => description;
            set { description = value; NotifyPropertyChanged(); }
        }

        private DateTime datePublication = DateTime.Today;
        public DateTime DatePublication
        {
            get => datePublication;
            set { datePublication = value; NotifyPropertyChanged(); }
        }

        private string adresseNumero = string.Empty;
        public string AdresseNumero
        {
            get => adresseNumero;
            set { adresseNumero = value; NotifyPropertyChanged(); }
        }

        private string adresseRue = string.Empty;
        public string AdresseRue
        {
            get => adresseRue;
            set { adresseRue = value; NotifyPropertyChanged(); }
        }

        private string adresseVille = string.Empty;
        public string AdresseVille
        {
            get => adresseVille;
            set { adresseVille = value; NotifyPropertyChanged(); }
        }

        private string adresseCodePostal = string.Empty;
        public string AdresseCodePostal
        {
            get => adresseCodePostal;
            set { adresseCodePostal = value; NotifyPropertyChanged(); }
        }

        private string adressePays = string.Empty;
        public string AdressePays
        {
            get => adressePays;
            set { adressePays = value; NotifyPropertyChanged(); }
        }

        public List<TypeBien> TypesDisponibles { get; set; } = new();

        private TypeBien? typeSelectionne;
        public TypeBien? TypeSelectionne
        {
            get => typeSelectionne;
            set { typeSelectionne = value; NotifyPropertyChanged(); }
        }

        public List<StatutBien> StatutsDisponibles { get; set; } = new();

        private StatutBien? statutSelectionne;
        public StatutBien? StatutSelectionne
        {
            get => statutSelectionne;
            set { statutSelectionne = value; NotifyPropertyChanged(); }
        }

        // Photos sélectionnées lors de la création 
        public System.Collections.ObjectModel.ObservableCollection<string> PhotosEnAttente { get; set; } = new();

        // Liste des propriétaires disponibles et celui sélectionné
        public List<ProprietaireInfo> ProprietairesDisponibles { get; set; } = new();

        private ProprietaireInfo? proprietaireSelectionne;
        public ProprietaireInfo? ProprietaireSelectionne
        {
            get => proprietaireSelectionne;
            set { proprietaireSelectionne = value; NotifyPropertyChanged(); }
        }

        public bool SauvegardeEffectuee { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
