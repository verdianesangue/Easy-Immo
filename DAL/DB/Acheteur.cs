using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Acheteur
{
    public int IdAch { get; set; }

    public decimal? BudgetMaxAch { get; set; }

    public string? ZoneSouhaiteAch { get; set; }

    public int? IdPersonne { get; set; }

    public virtual Personne? IdPersonneNavigation { get; set; }
}
