using DAL.DB;
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
        public static DAL.DB.Bien? GetBiens(int bienId)
        {
            using (var db = new DAL.DB.EasyImmo0Context())
            {
                return db.Biens.SingleOrDefault(a => a.IdBien == bienId);
            }
        }
    }
}