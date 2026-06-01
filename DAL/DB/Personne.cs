using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Personne
{
    public int IdPersonne { get; set; }

    public string NomPersonne { get; set; } = null!;

    public string PrenomPersonne { get; set; } = null!;

    public virtual ICollection<Acheteur> Acheteurs { get; set; } = new List<Acheteur>();

    public virtual ICollection<EffectuerOperationPersonne> EffectuerOperationPersonnes { get; set; } = new List<EffectuerOperationPersonne>();

    public virtual Employe? Employe { get; set; }

    public virtual ICollection<Locataire> Locataires { get; set; } = new List<Locataire>();

    public virtual ICollection<ParticiperActivitePersonne> ParticiperActivitePersonnes { get; set; } = new List<ParticiperActivitePersonne>();

    public virtual ICollection<Proprietaire> Proprietaires { get; set; } = new List<Proprietaire>();

    public virtual ICollection<MoyenContact> MoyenContacts { get; set; } = new List<MoyenContact>();
}
