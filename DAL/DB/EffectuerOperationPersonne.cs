using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class EffectuerOperationPersonne
{
    public int Id { get; set; }

    public int? IdContrat { get; set; }

    public int? IdPersonne { get; set; }

    public string? TypePersonne { get; set; }

    public virtual Contrat? IdContratNavigation { get; set; }

    public virtual Personne? IdPersonneNavigation { get; set; }
}
