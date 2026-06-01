using BU.Services;
using EasyImmoMaui.ViewModels;
using Microcharts;
using SkiaSharp;

namespace EasyImmoMaui.Views;

public partial class TableauDeBordView : ContentView
{
    private TableauDeBordViewModel tableauVM;

    public TableauDeBordView()
    {
        InitializeComponent();
        tableauVM = new TableauDeBordViewModel();
        this.BindingContext = tableauVM;
        ChargerDonnees();
    }

    private void ChargerDonnees()
    {
        var moisActuel   = DateTime.Now;
        var moisPrecedent = DateTime.Now.AddMonths(-1);

        // Statistiques réelles
        tableauVM.NombreBiens          = BienService.CompterBiens();
        tableauVM.NombreClients        = PersonneServices.CompterClients();
        tableauVM.NombreVisites        = ActiviteServices.CompterVisites();
        tableauVM.NombreContrats       = ContratService.CompterContrats();
        tableauVM.NombreProprietaires  = PersonneServices.CompterProprietaires();
        tableauVM.NombreLocataires     = PersonneServices.CompterLocataires();
        tableauVM.NombreAcheteurs      = PersonneServices.CompterAcheteurs();
        tableauVM.NombreBiensVendus    = BienService.CompterBiensVendus();
        tableauVM.NombreBiensLoues     = BienService.CompterBiensLoues();

        ChargerListes();

        // Tendances réelles (comparaison mois actuel vs mois précédent)
        int diffBiens = BienService.CompterBiensParMois(moisActuel)
                      - BienService.CompterBiensParMois(moisPrecedent);
        int diffVisites = ActiviteServices.CompterVisitesParMois(moisActuel)
                        - ActiviteServices.CompterVisitesParMois(moisPrecedent);
        int diffContrats = ContratService.CompterContratsParMois(moisActuel)
                         - ContratService.CompterContratsParMois(moisPrecedent);

        tableauVM.TendanceBiens          = FormatTendance(diffBiens);
        tableauVM.TendanceClients        = "—";
        tableauVM.TendanceVisites        = FormatTendance(diffVisites);
        tableauVM.TendanceContrats       = FormatTendance(diffContrats);
        tableauVM.TendanceProprietaires  = "—";

        // Création du graphique en courbe pour l'évolution des ventes
        var entriesVentes = new[]
        {
            new ChartEntry(8)  { Label = "Jan", ValueLabel = "8",  Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(12) { Label = "Fév", ValueLabel = "12", Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(10) { Label = "Mar", ValueLabel = "10", Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(15) { Label = "Avr", ValueLabel = "15", Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(14) { Label = "Mai", ValueLabel = "14", Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(18) { Label = "Jun", ValueLabel = "18", Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(17) { Label = "Jul", ValueLabel = "17", Color = SKColor.Parse("#5B4FE9") }
        };

        tableauVM.GraphiqueVentes = new LineChart
        {
            Entries = entriesVentes,
            LineMode = LineMode.Straight,
            LineSize = 4,
            PointMode = PointMode.Circle,
            PointSize = 12,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 24,
            LabelColor = SKColor.Parse("#666666"),
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            Margin = 20,
            LineAreaAlpha = 0
        };

        // Création du graphique pour la répartition des biens
        var entriesRepartition = new[]
        {
            new ChartEntry(BienService.CompterBiensParType(1)) { Label = "Apparts",  ValueLabel = BienService.CompterBiensParType(1).ToString(), Color = SKColor.Parse("#5B4FE9") },
            new ChartEntry(BienService.CompterBiensParType(2)) { Label = "Maisons",  ValueLabel = BienService.CompterBiensParType(2).ToString(), Color = SKColor.Parse("#3EBD7E") },
            new ChartEntry(BienService.CompterBiensParType(4)) { Label = "Studios",  ValueLabel = BienService.CompterBiensParType(4).ToString(), Color = SKColor.Parse("#B83EC9") },
            new ChartEntry(BienService.CompterBiensParType(3)) { Label = "Terrains", ValueLabel = BienService.CompterBiensParType(3).ToString(), Color = SKColor.Parse("#1FB6E8") }
        };

        tableauVM.GraphiqueRepartition = new DonutChart
        {
            Entries = entriesRepartition,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 24,
            LabelMode = LabelMode.LeftAndRight,
            HoleRadius = 0.5f,
            GraphPosition = GraphPosition.Center
        };

    }

    private void ChargerListes()
    {
        try { tableauVM.BiensVendus = new System.Collections.ObjectModel.ObservableCollection<DAL.DB.Bien>(BienService.GetBiensVendus()); }
        catch { }

        try { tableauVM.BiensLoues = new System.Collections.ObjectModel.ObservableCollection<DAL.DB.Bien>(BienService.GetBiensLoues()); }
        catch { }

        try { tableauVM.ListeProprietaires = new System.Collections.ObjectModel.ObservableCollection<ProprietaireInfo>(BienService.GetAllProprietaires()); }
        catch { }

        try { tableauVM.ListeLocataires = new System.Collections.ObjectModel.ObservableCollection<ClientEntity>(PersonneServices.GetListeLocataires()); }
        catch { }

        try { tableauVM.ListeAcheteurs = new System.Collections.ObjectModel.ObservableCollection<ClientEntity>(PersonneServices.GetListeAcheteurs()); }
        catch { }
    }

    private static string FormatTendance(int diff) =>
        diff > 0 ? $"↑ +{diff}" : diff < 0 ? $"↓ {diff}" : "→ 0";
}
