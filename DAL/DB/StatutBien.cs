using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class StatutBien
{
    public int IdSta { get; set; }

    public string? LibelleSta { get; set; }

    public DateTime? DateMiseJourSta { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual HistoriqueStatusBien? HistoriqueStatusBien { get; set; }
}
