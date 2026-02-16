using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Adresse
{
    public int IdAdresse { get; set; }

    public string? Rue { get; set; }

    public string? Numero { get; set; }

    public string? Ville { get; set; }

    public string? CodePostal { get; set; }

    public string? Pays { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();
}
