using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ContratDetailViewModel : INotifyPropertyChanged
    {
        public string Description { get; set; } = string.Empty;
        public string TypeContrat { get; set; } = string.Empty;
        public string DateContrat { get; set; } = string.Empty;
        public string NomBien { get; set; } = "—";
        public string Adresse { get; set; } = "—";
        public string Prix { get; set; } = "—";
        public string NomAgent { get; set; } = "—";
        public string LabelParticipant { get; set; } = "Acheteur";
        public string NomParticipantPrincipal { get; set; } = "—";
        public string NomProprietaire { get; set; } = "—";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
