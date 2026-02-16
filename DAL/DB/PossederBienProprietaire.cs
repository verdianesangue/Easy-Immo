using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class PossederBienProprietaire
{
    public int Id { get; set; }

    public int? IdBien { get; set; }

    public int? IdPro { get; set; }

    public DateTime? AnneeAcquisition { get; set; }

    public virtual Bien? IdBienNavigation { get; set; }

    public virtual Proprietaire? IdProNavigation { get; set; }
}
