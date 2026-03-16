using DAL.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BU.Services
{
    public class BienService
    {
        //  Récupérer tous les biens
        public static List<DAL.DB.Bien> GetBiens()
        {
            using (var db = new DAL.DB.EasyImmo0Context())
            {
                return db.Biens.ToList();
            }
        }
        public static DAL.DB.Bien? GetBienDetail(int bienId)
        {
            using (var db = new DAL.DB.EasyImmo0Context())
            {
                return db.Biens
                    .Include(b => b.IdTyNavigation)
                    .Include(b => b.HistoriqueStatusBien)
                        .ThenInclude(h => h.Id1)
                    .SingleOrDefault(b => b.IdBien == bienId);
            }
        }
    }
}