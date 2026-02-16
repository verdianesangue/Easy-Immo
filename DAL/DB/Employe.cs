using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Employe
{
    public int IdEmp { get; set; }

    public string? RoleEmp { get; set; }

    public int IdPersonne { get; set; }

    public virtual Personne IdEmpNavigation { get; set; } = null!;

    public virtual ICollection<OperationImmobiliere> OperationImmobilieres { get; set; } = new List<OperationImmobiliere>();
}
