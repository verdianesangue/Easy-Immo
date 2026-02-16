using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Locataire
{
    public int IdLocataire { get; set; }

    public decimal? MontantMax { get; set; }

    public int IdPersonne { get; set; }

    public virtual Personne IdLocataireNavigation { get; set; } = null!;
}
