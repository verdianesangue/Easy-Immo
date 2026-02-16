using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Planning
{
    public int IdPlanning { get; set; }

    public string? DescriptionPlanning { get; set; }

    public int? IdActivite { get; set; }

    public virtual Activite? IdActiviteNavigation { get; set; }
}
