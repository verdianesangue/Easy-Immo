using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Contrat
{
    public int IdContrat { get; set; }

    public string? TypeContrat { get; set; }

    public string? DescriptionOperation { get; set; }

    public DateTime? DateOperation { get; set; }

    public int? IdEmp { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual ICollection<EffectuerOperationPersonne> EffectuerOperationPersonnes { get; set; } = new List<EffectuerOperationPersonne>();

    public virtual Employe? IdEmpNavigation { get; set; }
}
