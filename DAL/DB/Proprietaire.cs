using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Proprietaire
{
    public int IdPro { get; set; }

    public string? TypePro { get; set; }

    public int? IdPersonnes { get; set; }

    public virtual Personne IdProNavigation { get; set; } = null!;

    public virtual ICollection<PossederBienProprietaire> PossederBienProprietaires { get; set; } = new List<PossederBienProprietaire>();
}
