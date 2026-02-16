using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class EffectuerOperationPersonne
{
    public int Id { get; set; }

    public int? IdOperation { get; set; }

    public int? IdPersonne { get; set; }

    public string? TypePersonne { get; set; }

    public virtual Personne Id1 { get; set; } = null!;

    public virtual OperationImmobiliere IdNavigation { get; set; } = null!;
}
