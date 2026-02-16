using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class DocumentBien
{
    public int IdDoc { get; set; }

    public string? DescriptionDoc { get; set; }

    public int? IdBien { get; set; }

    public virtual Bien? IdBienNavigation { get; set; }
}
