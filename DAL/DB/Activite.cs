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

    public int IdPlanning { get; set; }

    public virtual ParticiperActivitePersonne? ParticiperActivitePersonne { get; set; }

    public virtual ICollection<Planning> Plannings { get; set; } = new List<Planning>();
}
