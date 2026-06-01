using BU;
using BU.Services;
using DAL.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ContratFormulaireViewModel : INotifyPropertyChanged
    {
        // Types de contrat disponibles
        public List<TypeContrat> TypesContrat { get; } = new List<TypeContrat>
        {
            TypeContrat.Vente,
            TypeContrat.Location
        };

        private TypeContrat typeContrat = TypeContrat.Vente;
        public TypeContrat TypeContrat
        {
            get => typeContrat;
            set
            {
                typeContrat = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LabelParticipant));
                NotifyPropertyChanged(nameof(InfoStatut));
            }
        }

       
        public string LabelParticipant =>
            TypeContrat == TypeContrat.Vente ? "Acheteur" : "Locataire";

        // Message d'info statut automatique
        public string InfoStatut =>
            TypeContrat == TypeContrat.Vente
                ? "Le bien sera automatiquement passé en Vendu."
                : "Le bien sera automatiquement passé en À louer.";

        private string description = string.Empty;
        public string Description
        {
            get => description;
            set { description = value; NotifyPropertyChanged(); }
        }

        private DateTime dateOperation = DateTime.Today;
        public DateTime DateOperation
        {
            get => dateOperation;
            set { dateOperation = value; NotifyPropertyChanged(); }
        }

        // Bien concerné
        public List<Bien> BiensDisponibles { get; set; } = new();
        private Bien? bienSelectionne;
        public Bien? BienSelectionne
        {
            get => bienSelectionne;
            set { bienSelectionne = value; NotifyPropertyChanged(); }
        }

        // Agent immobilier
        public List<AgentInfo> AgentsDisponibles { get; set; } = new();
        private AgentInfo? agentSelectionne;
        public AgentInfo? AgentSelectionne
        {
            get => agentSelectionne;
            set { agentSelectionne = value; NotifyPropertyChanged(); }
        }

      
        public List<ProprietaireInfo> AcheteursDisponibles { get; set; } = new();

        public List<ProprietaireInfo> LocatairesDisponibles { get; set; } = new();

        private ProprietaireInfo? participantSelectionne;
        public ProprietaireInfo? ParticipantSelectionne
        {
            get => participantSelectionne;
            set { participantSelectionne = value; NotifyPropertyChanged(); }
        }

        // Propriétaire
        public List<ProprietaireInfo> ProprietairesDisponibles { get; set; } = new();
        private ProprietaireInfo? proprietaireSelectionne;
        public ProprietaireInfo? ProprietaireSelectionne
        {
            get => proprietaireSelectionne;
            set { proprietaireSelectionne = value; NotifyPropertyChanged(); }
        }

        private string titre = "Nouveau contrat";
        public string Titre
        {
            get => titre;
            set { titre = value; NotifyPropertyChanged(); }
        }

        public bool SauvegardeEffectuee { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
