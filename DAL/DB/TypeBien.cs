using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class TypeBien
{
    public int IdTy { get; set; }

    public string? DescriptionType { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();
}
