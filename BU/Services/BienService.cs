using BU.Services;
using DAL.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BU.Services
{ 
    public class BienService
    {
        // Récupérer tous les biens avec leur photo principale
        public static List<Bien> GetBiens()
        {
            using var db = new EasyImmo0Context();
            var biens = db.Biens
                .Include(b => b.IdAdresseNavigation)
                .Include(b => b.IdTyNavigation)
                .Include(b => b.IdStatusNavigation)
                .ToList();
           
            // Charger la première photo de chaque bien
            var docs = db.DocumentBiens.ToList();
            foreach (var bien in biens)
            {
                var photo = docs.FirstOrDefault(d =>
                    d.IdBien == bien.IdBien &&
                    d.DescriptionDoc != null &&
                    d.DescriptionDoc.StartsWith("PHOTO:"));
                bien.CheminPhoto = photo?.DescriptionDoc?.Substring(6);
            }
            return biens;
        }

        // Récupere uniquement les biens disponibles
        public static List<Bien> GetBiensDisponibles()
        {
            using var db = new EasyImmo0Context();
            return db.Biens
                .Include(b => b.IdAdresseNavigation)
                .Include(b => b.IdTyNavigation)
                .Include(b => b.IdStatusNavigation)
                .Where(b => b.IdStatusNavigation == null ||
                           (b.IdStatusNavigation.LibelleSta != null &&
                            b.IdStatusNavigation.LibelleSta.ToLower() != "vendu" &&
                            b.IdStatusNavigation.LibelleSta.ToLower() != "louer"))
                .ToList();
        }

        public static List<Bien> GetBiensVendus()
        {
            using var db = new EasyImmo0Context();
            var idStatut = db.StatutBiens
                .FirstOrDefault(s => s.LibelleSta != null && s.LibelleSta.ToLower() == "vendu")?.IdSta;
            if (idStatut == null) return new List<Bien>();
            return db.Biens
                .Include(b => b.IdTyNavigation)
                .Include(b => b.IdStatusNavigation)
                .Where(b => b.IdStatus == idStatut)
                .ToList();
        }

        public static List<Bien> GetBiensLoues()
        {
            using var db = new EasyImmo0Context();
            var idStatut = db.StatutBiens
                .FirstOrDefault(s => s.LibelleSta != null && s.LibelleSta.ToLower() == "louer")?.IdSta;
            if (idStatut == null) return new List<Bien>();
            return db.Biens
                .Include(b => b.IdTyNavigation)
                .Include(b => b.IdStatusNavigation)
                .Where(b => b.IdStatus == idStatut)
                .ToList();
        }

        public static int CompterBiensVendus()
        {
            using var db = new EasyImmo0Context();
            var idStatut = db.StatutBiens
                .FirstOrDefault(s => s.LibelleSta != null && s.LibelleSta.ToLower() == "vendu")?.IdSta;
            return idStatut == null ? 0 : db.Biens.Count(b => b.IdStatus == idStatut);
        }

        public static int CompterBiensLoues()
        {
            using var db = new EasyImmo0Context();
            var idStatut = db.StatutBiens
                .FirstOrDefault(s => s.LibelleSta != null && s.LibelleSta.ToLower() == "louer")?.IdSta;
            return idStatut == null ? 0 : db.Biens.Count(b => b.IdStatus == idStatut);
        }

        // Methode pour compter le nombre total de biens
        public static int CompterBiens()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Biens.Count();
            }
        }

        // Methode pour compter les biens par mois de publication
        public static int CompterBiensParMois(DateTime date)
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Biens.Count(b => b.DatePublicationBien.HasValue &&
                                           b.DatePublicationBien.Value.Month == date.Month &&
                                           b.DatePublicationBien.Value.Year == date.Year);
            }
        }
       

        // Methode pour compter les biens par type
        public static int CompterBiensParType(int idType)
        {
            var nombreApartements = 0;
            var nombreMaisons = 0;
            var nombreTerrains = 0;
            var nombreStudios = 0;
            using (var db = new EasyImmo0Context())
            {
               if (idType == 1)
                    nombreApartements = db.Biens.Count(b => b.IdTy == 1);
                else if (idType == 2)
                    nombreMaisons = db.Biens.Count(b => b.IdTy == 2);
                else if (idType == 3)
                    nombreTerrains = db.Biens.Count(b => b.IdTy == 3);
                else if (idType == 4)
                    nombreStudios = db.Biens.Count(b => b.IdTy == 4);
                return idType switch
                {
                    1 => nombreApartements,
                    2 => nombreMaisons,
                    3 => nombreTerrains,
                    4 => nombreStudios,
                    _ => 0
                };
            }
        }

        // Rechercher des biens par mot-clé 
        public static List<Bien> RechercherBiens(string motCle)
        {
            using (var db = new EasyImmo0Context())
            {
                if (string.IsNullOrWhiteSpace(motCle))
                    return GetBiens();

                motCle = motCle.ToLower();

                return db.Biens
                    .Include(b => b.IdAdresseNavigation)
                    .Include(b => b.IdTyNavigation)
                    .Include(b => b.HistoriqueStatusBien)
                    .Include(b => b.IdStatusNavigation)
                    .Where(b =>
                        (b.NomBien != null && b.NomBien.ToLower().Contains(motCle)) ||
                        (b.DescriptionBien != null && b.DescriptionBien.ToLower().Contains(motCle)) ||
                        (b.IdAdresseNavigation != null && b.IdAdresseNavigation.Ville != null &&
                         b.IdAdresseNavigation.Ville.ToLower().Contains(motCle))
                    )
                    .ToList();
            }
        }

        // Récupérer un bien par son Id
        public static Bien? GetBienById(int id)
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Biens
                    .Include(b => b.IdAdresseNavigation)
                    .Include(b => b.IdTyNavigation)

                    .FirstOrDefault(b => b.IdBien == id);
            }
        }

        // Créer un nouveau bien 
        public static bool AjouterBien(Bien bien)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    db.Biens.Add(bien);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Transitions autorisées entre statuts 
        private static readonly Dictionary<string, List<string>> TransitionsAutorisees = new()
        {
            { "a vendre",      new() { "sous-option", "en rénovation", "a louer" } },
            { "sous-option",   new() { "a vendre", "vendu" } },
            { "a louer",       new() { "en rénovation", "a vendre", "louer" } },
            { "en rénovation", new() { "a vendre", "a louer" } },
            { "louer",         new() { } },
            { "vendu",         new() { } }
        };

        // Mettre à jour un bien existant
        public static bool ModifierBien(Bien bien)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var bienExistant = db.Biens
                        .Include(b => b.IdStatusNavigation)
                        .FirstOrDefault(b => b.IdBien == bien.IdBien);
                    if (bienExistant == null) return false;

                    // Validation de la transition de statut
                    bool statutAChange = bienExistant.IdStatus != bien.IdStatus;
                    if (statutAChange && bienExistant.IdStatusNavigation != null)
                    {
                        var statutActuel = bienExistant.IdStatusNavigation.LibelleSta?.ToLower() ?? string.Empty;
                        var nouveauStatut = db.StatutBiens
                            .FirstOrDefault(s => s.IdSta == bien.IdStatus)?.LibelleSta?.ToLower() ?? string.Empty;

                        if (TransitionsAutorisees.TryGetValue(statutActuel, out var autorisees))
                        {
                            if (!autorisees.Contains(nouveauStatut))
                            {
                                string msg = statutActuel == "vendu"
                                    ? "Ce bien est vendu — son statut ne peut plus être modifié."
                                    : statutActuel == "louer"
                                        ? "Ce bien est loué — son statut ne peut plus être modifié."
                                        : $"Transition impossible : de \"{statutActuel}\" vers \"{nouveauStatut}\"";
                                throw new Common.Exception.SuppressionImpossibleException(msg);
                            }
                        }
                    }

                    bienExistant.NomBien = bien.NomBien;
                    bienExistant.PrixBien = bien.PrixBien;
                    bienExistant.SurfacesBien = bien.SurfacesBien;
                    bienExistant.DescriptionBien = bien.DescriptionBien;
                    bienExistant.DatePublicationBien = bien.DatePublicationBien;
                    bienExistant.IdTy = bien.IdTy;
                    bienExistant.IdAdresse = bien.IdAdresse;
                    bienExistant.IdStatus = bien.IdStatus;

                    // Si le statut a changer, on met a jour la date
                    if (statutAChange)
                        bienExistant.DateChangementStatus = DateTime.Now;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Common.Exception.SuppressionImpossibleException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }

        // supprimer un bien 
        public static bool SupprimerBien(int id)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var bien = db.Biens.FirstOrDefault(b => b.IdBien == id);
                    if (bien == null) return false;

                    db.Biens.Remove(bien);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex) 
            {
                return false;
            }
        }




        // Pour les listes déroulantes des types de biens
        public static List<TypeBien> GetAllTypes()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.TypeBiens.ToList();
            }
        }

        // Tout les statuts 
   
        public static List<StatutBien> GetAllStatuts()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.StatutBiens.ToList();
            }
        }

        // Toutes les adresses 
       // public static List<Adresse> GetAllAdresses()
        //{
            // using (var db = new EasyImmo0Context())
           // {
              //  return db.Adresses.ToList();
            //}
        //}

        // Ajouter une adresse et retourner son Id
        public static int? AjouterAdresse(Adresse adresse)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    db.Adresses.Add(adresse);
                    db.SaveChanges();
                    return adresse.IdAdresse;
                }
            }
            catch
            {
                return null;
            }
        }
        // Récupère toutes les activités d'un bien avec leurs participants 
        public static List<ActiviteInfo> GetActivitesDuBienAvecParticipants(int idBien)
        {
            using var db = new EasyImmo0Context();

            var activites = db.Activites
                .Include(a => a.IdTypeNavigation)
                .Include(a => a.ParticiperActivitePersonnes)
                    .ThenInclude(p => p.IdPersonneNavigation)
                .Where(a => a.IdBien == idBien)
                .OrderByDescending(a => a.DateActivite)
                .ToList();

            return activites.Select(a => new ActiviteInfo
            {
                LibelleActivite = a.LibelleActivite ?? string.Empty,
                DateActivite = a.DateActivite,
                NomType = a.IdTypeNavigation?.DescriptionType ?? "Inconnu",
                Participants = string.Join(", ", a.ParticiperActivitePersonnes
                    .Where(p => p.IdPersonneNavigation != null)
                    .Select(p => $"{p.IdPersonneNavigation!.PrenomPersonne} {p.IdPersonneNavigation.NomPersonne}"))
            }).ToList();
        }

        // Récupère tous les documents d'un bien
        public static List<DocumentBien> GetDocuments(int idBien)
        {
            using var db = new EasyImmo0Context();
            return db.DocumentBiens
                .Where(d => d.IdBien == idBien)
                .ToList();
        }

        // Ajoute un document à un bien
        public static void AjouterDocument(int idBien, string description)
        {
            using var db = new EasyImmo0Context();
            db.DocumentBiens.Add(new DocumentBien
            {
                IdBien = idBien,
                DescriptionDoc = description
            });
            db.SaveChanges();
        }

        // Supprime un document
        public static void SupprimerDocument(int idDoc)
        {
            using var db = new EasyImmo0Context();
            var doc = db.DocumentBiens.FirstOrDefault(d => d.IdDoc == idDoc);
            if (doc != null)
            {
                db.DocumentBiens.Remove(doc);
                db.SaveChanges();
            }
        }

        // Récupère tous les propriétaires avec leurs informations de personne
        public static List<ProprietaireInfo> GetAllProprietaires()
        {
            using var db = new EasyImmo0Context();
            return db.Proprietaires
                .Include(p => p.IdPersonnesNavigation)
                .Where(p => p.IdPersonnesNavigation != null)
                .Select(p => new ProprietaireInfo
                {
                    IdPro = p.IdPro,
                    IdPersonne = p.IdPersonnes ?? 0,
                    NomComplet = p.IdPersonnesNavigation!.PrenomPersonne + " " + p.IdPersonnesNavigation.NomPersonne,
                    TypePro = p.TypePro ?? "Particulier"
                })
                .ToList();
        }

        // Récupère le propriétaire actuel d'un bien
        public static ProprietaireInfo? GetProprietaireDuBien(int idBien)
        {
            using var db = new EasyImmo0Context();
            var lien = db.PossederBienProprietaires
                .Include(pb => pb.IdProNavigation)
                    .ThenInclude(p => p!.IdPersonnesNavigation)
                .FirstOrDefault(pb => pb.IdBien == idBien);

            if (lien?.IdProNavigation?.IdPersonnesNavigation == null) return null;

            return new ProprietaireInfo
            {
                IdPro = lien.IdProNavigation.IdPro,
                IdPersonne = lien.IdProNavigation.IdPersonnes ?? 0,
                NomComplet = lien.IdProNavigation.IdPersonnesNavigation.PrenomPersonne + " "
                           + lien.IdProNavigation.IdPersonnesNavigation.NomPersonne,
                TypePro = lien.IdProNavigation.TypePro ?? "Particulier"
            };
        }

        // Associe un propriétaire à un bien 
        public static void AssocierProprietaire(int idBien, int idPro)
        {
            using var db = new EasyImmo0Context();

            var lienExistant = db.PossederBienProprietaires
                .FirstOrDefault(pb => pb.IdBien == idBien);

            if (lienExistant != null)
                db.PossederBienProprietaires.Remove(lienExistant);

            db.PossederBienProprietaires.Add(new PossederBienProprietaire
            {
                IdBien = idBien,
                IdPro = idPro,
                AnneeAcquisition = DateTime.Today
            });
            db.SaveChanges();
        }

        // Modifier une adresse existante
        public static bool ModifierAdresse(Adresse adresse)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var adresseExistante = db.Adresses.FirstOrDefault(a => a.IdAdresse == adresse.IdAdresse);
                    if (adresseExistante == null) return false;

                    adresseExistante.Numero = adresse.Numero;
                    adresseExistante.Rue = adresse.Rue;
                    adresseExistante.Ville = adresse.Ville;
                    adresseExistante.CodePostal = adresse.CodePostal;
                    adresseExistante.Pays = adresse.Pays;

                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }

    public class ProprietaireInfo
    {
        public int IdPro { get; set; }
        public int IdPersonne { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string TypePro { get; set; } = string.Empty;

        public override string ToString() => NomComplet;
    }
}