using DAL.DB;

namespace BU.Services
{
    public class ActiviteServices
    {
        public static List<Activite> GetActivitesByBien(int bienId)
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Activites
                    .Where(a => a.IdBien == bienId)
                    .OrderByDescending(a => a.DateActivite)
                    .ToList();
            }
        }
    }
}
