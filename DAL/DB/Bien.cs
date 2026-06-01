using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Bien
{
    public int IdBien { get; set; }

    public decimal PrixBien { get; set; }

    public decimal? SurfacesBien { get; set; }

    public string? DescriptionBien { get; set; }

    public DateTime? DatePublicationBien { get; set; }

    public int? IdContrat { get; set; }

    public int? IdTy { get; set; }

    public int? IdAdresse { get; set; }

    public string? NomBien { get; set; }

    public int? IdStatus { get; set; }

    public DateTime? DateChangementStatus { get; set; }

    public virtual ICollection<DocumentBien> DocumentBiens { get; set; } = new List<DocumentBien>();

    public virtual HistoriqueStatusBien? HistoriqueStatusBien { get; set; }

    public virtual Adresse? IdAdresseNavigation { get; set; }

    public virtual Contrat? IdContratNavigation { get; set; }

    public virtual StatutBien? IdStatusNavigation { get; set; }

    public virtual TypeBien? IdTyNavigation { get; set; }

    public virtual ICollection<PossederBienProprietaire> PossederBienProprietaires { get; set; } = new List<PossederBienProprietaire>();

    public virtual ICollection<Activite> Activites { get; set; } = new List<Activite>();
}
