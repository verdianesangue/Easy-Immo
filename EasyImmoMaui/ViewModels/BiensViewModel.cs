using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class BiensViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Bien> Biens { get; set; } = new ObservableCollection<Bien>();

        private string texteRecherche = string.Empty;
        public string TexteRecherche
        {
            get => texteRecherche;
            set { texteRecherche = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
