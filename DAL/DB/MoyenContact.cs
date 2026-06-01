using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class MoyenContact
{
    public int IdContact { get; set; }

    public string TypeContact { get; set; } = null!;

    public string Valeur { get; set; } = null!;

    public int? IdPersonne { get; set; }

    public virtual Personne? IdPersonneNavigation { get; set; }
}
