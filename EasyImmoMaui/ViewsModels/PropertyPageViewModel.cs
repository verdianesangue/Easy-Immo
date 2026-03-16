using DAL.DB;
using System.Collections.ObjectModel;

namespace EasyImmoMaui.ViewsModels
{
    public class PropertyPageViewModel
    {
        public int IdBien { get; set; }
        public string TypeBien { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ObservableCollection<Activite> Activites { get; set; } = new();
    }
}
