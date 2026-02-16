using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class ParticiperActivitePersonne
{
    public int Id { get; set; }

    public int? IdActivite { get; set; }

    public int? IdPersonne { get; set; }

    public string? But { get; set; }

    public virtual Personne Id1 { get; set; } = null!;

    public virtual Activite IdNavigation { get; set; } = null!;
}
