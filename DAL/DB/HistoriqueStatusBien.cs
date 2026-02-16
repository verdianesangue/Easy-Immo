using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class HistoriqueStatusBien
{
    public int Id { get; set; }

    public int? IdSta { get; set; }

    public int? IdBien { get; set; }

    public int? NombreBienVisite { get; set; }

    public int? NombreBienVendu { get; set; }

    public virtual StatutBien Id1 { get; set; } = null!;

    public virtual Bien IdNavigation { get; set; } = null!;
}
