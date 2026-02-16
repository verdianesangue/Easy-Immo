using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Personne
{
    public int IdPersonne { get; set; }

    public string NomPersonne { get; set; } = null!;

    public string PrenomPersonne { get; set; } = null!;

    public virtual Acheteur? Acheteur { get; set; }

    public virtual EffectuerOperationPersonne? EffectuerOperationPersonne { get; set; }

    public virtual Employe? Employe { get; set; }

    public virtual Locataire? Locataire { get; set; }

    public virtual ParticiperActivitePersonne? ParticiperActivitePersonne { get; set; }

    public virtual Proprietaire? Proprietaire { get; set; }
}
