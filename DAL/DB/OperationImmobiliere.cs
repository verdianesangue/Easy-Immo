using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class OperationImmobiliere
{
    public int IdOperation { get; set; }

    public string? DescriptionOperation { get; set; }

    public DateTime? DateOperation { get; set; }

    public int? IdEmp { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual EffectuerOperationPersonne? EffectuerOperationPersonne { get; set; }

    public virtual Employe? IdEmpNavigation { get; set; }
}
