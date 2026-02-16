using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class StatutBien
{
    public int IdSta { get; set; }

    public string? LibelleSta { get; set; }

    public DateTime? DateMiseJourSta { get; set; }

    public virtual HistoriqueStatusBien? HistoriqueStatusBien { get; set; }
}
