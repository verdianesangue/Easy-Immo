using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.DB
{
    public partial class Bien
    {
        [NotMapped]
        public string? CheminPhoto { get; set; }
    }
}
