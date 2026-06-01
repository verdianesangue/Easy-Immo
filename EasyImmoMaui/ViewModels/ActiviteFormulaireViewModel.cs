using BU;
using DAL.DB;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    // Classe d'affichage pour les types d'activité
    public class TypeActiviteAffichage
    {
        public TypeActiviteEnum Type { get; set; }
        public string Nom { get; set; } = string.Empty;
        public override string ToString() => Nom;
    }

    public class ActiviteFormulaireViewModel : INotifyPropertyChanged
    {
        // Titre dynamique de la popup
        private string titrePopup = "Ajouter une activité";
        public string TitrePopup
        {
            get => titrePopup;
            set { titrePopup = value; OnPropertyChanged(); }
        }

        // Champs du formulaire
        private string libelle = string.Empty;
        public string Libelle
        {
            get => libelle;
            set { libelle = value; OnPropertyChanged(); }
        }

        private DateTime dateActivite = DateTime.Today;
        public DateTime DateActivite
        {
            get => dateActivite;
            set { dateActivite = value; OnPropertyChanged(); }
        }

        private TimeSpan heureActivite = new TimeSpan(9, 0, 0);
        public TimeSpan HeureActivite
        {
            get => heureActivite;
            set { heureActivite = value; OnPropertyChanged(); }
        }

        private TimeSpan dureeActivite = new TimeSpan(1, 0, 0);
        public TimeSpan DureeActivite
        {
            get => dureeActivite;
            set { dureeActivite = value; OnPropertyChanged(); }
        }

       
        public ObservableCollection<TypeActiviteAffichage> TypesDisponibles { get; set; } = new();

        private TypeActiviteAffichage? typeSelectionne;
        public TypeActiviteAffichage? TypeSelectionne
        {
            get => typeSelectionne;
            set { typeSelectionne = value; OnPropertyChanged(); }
        }

        // Liste des biens pour le Picker
        public ObservableCollection<Bien> BiensDisponibles { get; set; } = new();

        private Bien? bienSelectionne;
        public Bien? BienSelectionne
        {
            get => bienSelectionne;
            set { bienSelectionne = value; OnPropertyChanged(); }
        }

        public bool SauvegardeEffectuee { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
