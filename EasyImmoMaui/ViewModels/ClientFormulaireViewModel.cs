using BU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ClientFormulaireViewModel : INotifyPropertyChanged
    {
        // Liste des types de propriétaire pour le Picker
        public List<TypeProprietaire> TypesProprietaire { get; } = new List<TypeProprietaire>
        {
            TypeProprietaire.Particulier,
            TypeProprietaire.Societe
        };
        private string titre = "Ajouter un Client";
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

        private string prenom = string.Empty;
        public string Prenom
        {
            get => prenom;
            set { prenom = value; NotifyPropertyChanged(); }
        }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; NotifyPropertyChanged(); }
        }

        private string telephone = string.Empty;
        public string Telephone
        {
            get => telephone;
            set { telephone = value; NotifyPropertyChanged(); }
        }

        private bool estAcheteur;
        public bool EstAcheteur
        {
            get => estAcheteur;
            set { estAcheteur = value; NotifyPropertyChanged(); }
        }

        private bool estLocataire;
        public bool EstLocataire
        {
            get => estLocataire;
            set { estLocataire = value; NotifyPropertyChanged(); }
        }

        private bool estProprietaire;
        public bool EstProprietaire
        {
            get => estProprietaire;
            set { estProprietaire = value; NotifyPropertyChanged(); }
        }

        // Si acheteur, on affiche
        private decimal? budgetMaxAcheteur;
        public decimal? BudgetMaxAcheteur
        {
            get => budgetMaxAcheteur;
            set { budgetMaxAcheteur = value; NotifyPropertyChanged(); }
        }

        private string zoneSouhaiteeAcheteur = string.Empty;
        public string ZoneSouhaiteeAcheteur
        {
            get => zoneSouhaiteeAcheteur;
            set { zoneSouhaiteeAcheteur = value; NotifyPropertyChanged(); }
        }

        // Si locataire, on affiche
        private decimal? montantMaxLocataire;
        public decimal? MontantMaxLocataire
        {
            get => montantMaxLocataire;
            set { montantMaxLocataire = value; NotifyPropertyChanged(); }
        }

        // Si propriétaire, on affiche
        private TypeProprietaire typeProprietaire = TypeProprietaire.Particulier;
        public TypeProprietaire TypeProprietaire
        {
            get => typeProprietaire;
            set { typeProprietaire = value; NotifyPropertyChanged(); }
        }

        public bool SauvegardeEffectuee { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
