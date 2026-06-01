using System;

namespace DAL.DB;

public partial class NoteBien
{
    public int IdNote { get; set; }

    public int IdActivite { get; set; }

    public int IdBien { get; set; }

    public int IdPersonne { get; set; }

    public int Note { get; set; }

    public string? Commentaire { get; set; }

    public DateTime DateNote { get; set; }
}
