using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Activite
{
    public int IdActivite { get; set; }

    public string? LibelleActivite { get; set; }

    public TimeOnly? DureeActivite { get; set; }

    public DateTime? DateActivite { get; set; }

    public int IdBien { get; set; }

    public int IdType { get; set; }

    public int? IdPlanning { get; set; }

    public virtual Bien IdBienNavigation { get; set; } = null!;

    public virtual TypeActivite IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<ParticiperActivitePersonne> ParticiperActivitePersonnes { get; set; } = new List<ParticiperActivitePersonne>();

    public virtual ICollection<Planning> Plannings { get; set; } = new List<Planning>();
}
