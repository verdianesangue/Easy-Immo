using BU.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMaui.ViewModels
{
    public class ParametresViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EmployeEntity> employes = new();
        public ObservableCollection<EmployeEntity> Employes
        {
            get => employes;
            set { employes = value; OnPropertyChanged(); }
        }

        private string texteRecherche = string.Empty;
        public string TexteRecherche
        {
            get => texteRecherche;
            set { texteRecherche = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
