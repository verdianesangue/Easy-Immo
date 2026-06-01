using BU;
using BU.Services;
using Common.Exception;
using DAL.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BU.Services
{
    public class ContratService
    {
        public static int CompterContrats()
        {
            using var db = new EasyImmo0Context();
            return db.Contrats.Count();
        }

        public static int CompterContratsParMois(DateTime date)
        {
            using var db = new EasyImmo0Context();
            return db.Contrats.Count(c => c.DateOperation.HasValue
                && c.DateOperation.Value.Month == date.Month
                && c.DateOperation.Value.Year == date.Year);
        }

        // Récupère tous les contrats avec les informations associées
        public static List<ContratAffichage> GetContrats()
        {
            using var db = new EasyImmo0Context();

            var contrats = db.Contrats
                .Include(c => c.Biens)
                    .ThenInclude(b => b.IdAdresseNavigation)
                .ToList();

            var personnes = db.Personnes.ToList();
            var employes  = db.Employes.ToList();
            var effectuer = db.EffectuerOperationPersonnes.ToList();

            return contrats.Select(c =>
            {
                var participants = effectuer.Where(e => e.IdContrat == c.IdContrat).ToList();

                string NomParticipant(string type)
                {
                    var e = participants.FirstOrDefault(p => p.TypePersonne == type);
                    if (e == null) return "—";
                    var p = personnes.FirstOrDefault(pe => pe.IdPersonne == e.IdPersonne);
                    return p != null ? $"{p.PrenomPersonne} {p.NomPersonne}" : "—";
                }

                string NomAgent()
                {
                    if (c.IdEmp == null) return "—";
                    var emp = employes.FirstOrDefault(e => e.IdEmp == c.IdEmp);
                    if (emp == null) return "—";
                    var p = personnes.FirstOrDefault(pe => pe.IdPersonne == emp.IdPersonne);
                    return p != null ? $"{p.PrenomPersonne} {p.NomPersonne}" : "—";
                }

                return new ContratAffichage
                {
                    Contrat = c,
                    NomBien = c.Biens.FirstOrDefault()?.NomBien ?? "Aucun bien",
                    NomAgent = NomAgent(),
                    NomAcheteur = NomParticipant("Acheteur"),
                    NomLocataire = NomParticipant("Locataire"),
                    NomProprietaire = NomParticipant("Propriétaire")
                };
            }).ToList();
        }

        // Récupère les agents immobiliers 
        public static List<AgentInfo> GetAgents()
        {
            using var db = new EasyImmo0Context();
            return db.Employes
                .Include(e => e.IdEmpNavigation)
                    .ThenInclude(p => p.MoyenContacts)
                .Where(e => e.IdEmpNavigation != null)
                .ToList()
                .Select(e => new AgentInfo
                {
                    IdEmp = e.IdEmp,
                    NomComplet = e.IdEmpNavigation.PrenomPersonne + " " + e.IdEmpNavigation.NomPersonne,
                    Role = e.RoleEmp ?? string.Empty
                })
                .ToList();
        }

        // Récupère les personnes qui ont le rôle Locataire
        public static List<ProprietaireInfo> GetLocataires()
        {
            using var db = new EasyImmo0Context();
            return db.Locataires
                .Include(l => l.IdPersonneNavigation)
                .Where(l => l.IdPersonneNavigation != null)
                .Select(l => new ProprietaireInfo
                {
                    IdPro = l.IdLocataire,
                    IdPersonne = l.IdPersonne,
                    NomComplet = l.IdPersonneNavigation!.PrenomPersonne + " " + l.IdPersonneNavigation.NomPersonne,
                    TypePro = "Locataire"
                })
                .ToList();
        }

        // Récupère les détails complets d'un contrat pour la popup
        public static ContratDetail? GetContratDetail(int idContrat)
        {
            using var db = new EasyImmo0Context();

            var contrat = db.Contrats
                .Include(c => c.Biens).ThenInclude(b => b.IdAdresseNavigation)
                .Include(c => c.IdEmpNavigation).ThenInclude(e => e!.IdEmpNavigation)
                .FirstOrDefault(c => c.IdContrat == idContrat);

            if (contrat == null) return null;

            var personnes = db.Personnes.ToList();
            var effectuer = db.EffectuerOperationPersonnes
                .Where(e => e.IdContrat == idContrat).ToList();

            string NomParticipant(string type)
            {
                var e = effectuer.FirstOrDefault(p => p.TypePersonne == type);
                if (e == null) return "—";
                var p = personnes.FirstOrDefault(pe => pe.IdPersonne == e.IdPersonne);
                return p != null ? $"{p.PrenomPersonne} {p.NomPersonne}" : "—";
            }

            var bien = contrat.Biens.FirstOrDefault();
            var type = contrat.TypeContrat ?? "Vente";
            string participantType = type.ToLower() == "vente" ? "Acheteur" : "Locataire";

            return new ContratDetail
            {
                Description = contrat.DescriptionOperation ?? string.Empty,
                TypeContrat = type,
                DateContrat = contrat.DateOperation,
                NomBien = bien?.NomBien ?? "—",
                Adresse = bien?.IdAdresseNavigation != null
                    ? $"{bien.IdAdresseNavigation.Numero} {bien.IdAdresseNavigation.Rue}, {bien.IdAdresseNavigation.Ville}"
                    : "—",
                PrixBien = bien?.PrixBien,
                NomAgent = contrat.IdEmpNavigation?.IdEmpNavigation != null
                    ? $"{contrat.IdEmpNavigation.IdEmpNavigation.PrenomPersonne} {contrat.IdEmpNavigation.IdEmpNavigation.NomPersonne}"
                    : "—",
                LabelParticipant = participantType,
                NomParticipantPrincipal = NomParticipant(participantType),
                NomProprietaire = NomParticipant("Propriétaire")
            };
        }

        // Récupère les personnes qui ont le rôle Acheteur
        public static List<ProprietaireInfo> GetAcheteurs()
        {
            using var db = new EasyImmo0Context();
            return db.Acheteurs
                .Include(a => a.IdPersonneNavigation)
                .Where(a => a.IdPersonneNavigation != null)
                .Select(a => new ProprietaireInfo
                {
                    IdPro = a.IdAch,
                    IdPersonne = a.IdPersonne ?? 0,
                    NomComplet = a.IdPersonneNavigation!.PrenomPersonne + " " + a.IdPersonneNavigation.NomPersonne,
                    TypePro = "Acheteur"
                })
                .ToList();
        }

        // Récupère les personnes qui ont le rôle Propriétaire
        public static List<ProprietaireInfo> GetProprietaires()
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
                    TypePro = "Propriétaire"
                })
                .ToList();
        }

        // Ajoute un contrat et met à jour automatiquement le statut du bien
        public static void AjouterContrat(
            string description,
            DateTime date,
            int idBien,
            TypeContrat typeContrat,
            int? idEmp,
            int? idPersonneAcheteurOuLocataire,
            int? idPersonneProprietaire)
        {
            // vérifier que le bien n'est pas déjà vendu
            try
            {
                using var db = new EasyImmo0Context();
                var bien = db.Biens
                    .Include(b => b.IdStatusNavigation)
                    .FirstOrDefault(b => b.IdBien == idBien);

                if (bien == null)
                    throw new EntiteIntrouvableException("Ce bien n'existe plus.");

                var statut = bien.IdStatusNavigation?.LibelleSta?.ToLower() ?? string.Empty;
                if (statut == "vendu")
                    throw new SuppressionImpossibleException(
                        "Ce bien est déjà vendu. Son statut ne peut plus être modifié.");

                if (statut == "louer")
                    throw new SuppressionImpossibleException(
                        "Ce bien est déjà loué. Son statut ne peut plus être modifié.");

                if (bien.IdContrat != null)
                    throw new SuppressionImpossibleException(
                        "Ce bien est déjà lié à un contrat existant. Supprimez d'abord l'ancien contrat.");
            }
            catch (EntiteIntrouvableException) { throw; }
            catch (SuppressionImpossibleException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Erreur lors de la vérification du bien.", ex);
            }

            int idContrat;

            // créer le contrat
            try
            {
                using var db = new EasyImmo0Context();
                var contrat = new Contrat
                {
                    DescriptionOperation = description,
                    DateOperation = date,
                    TypeContrat = typeContrat.ToString(),
                    IdEmp = idEmp
                };
                db.Contrats.Add(contrat);
                db.SaveChanges();
                idContrat = contrat.IdContrat;
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible d'enregistrer le contrat.", ex);
            }

            // lier le bien et mettre à jour son statut selon le type de contrat
            try
            {
                using var db = new EasyImmo0Context();
                var bien = db.Biens.FirstOrDefault(b => b.IdBien == idBien);

               
                string libelleStatut = typeContrat == TypeContrat.Vente ? "vendu" : "louer";
                var nouveauStatut = db.StatutBiens
                    .FirstOrDefault(s => s.LibelleSta != null && s.LibelleSta.ToLower() == libelleStatut);

                if (nouveauStatut == null)
                    throw new EntiteIntrouvableException(
                        $"Le statut \"{libelleStatut}\" est introuvable dans la base de données. Veuillez l'ajouter via SSMS.");

                if (bien != null)
                {
                    bien.IdContrat = idContrat;
                    bien.IdStatus = nouveauStatut.IdSta;
                    bien.DateChangementStatus = DateTime.Now;
                    db.SaveChanges();
                }
            }
            catch (EntiteIntrouvableException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible de lier le bien au contrat.", ex);
            }

            // ajouter les participants
            try
            {
                using var db = new EasyImmo0Context();
                string typePersonne = typeContrat == TypeContrat.Vente ? "Acheteur" : "Locataire";

                if (idPersonneAcheteurOuLocataire.HasValue)
                {
                    db.EffectuerOperationPersonnes.Add(new EffectuerOperationPersonne
                    {
                        IdContrat = idContrat,
                        IdPersonne = idPersonneAcheteurOuLocataire.Value,
                        TypePersonne = typePersonne
                    });
                }
                if (idPersonneProprietaire.HasValue)
                {
                    db.EffectuerOperationPersonnes.Add(new EffectuerOperationPersonne
                    {
                        IdContrat = idContrat,
                        IdPersonne = idPersonneProprietaire.Value,
                        TypePersonne = "Propriétaire"
                    });
                }
                if (idPersonneAcheteurOuLocataire.HasValue || idPersonneProprietaire.HasValue)
                    db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible d'enregistrer les participants.", ex);
            }
        }

        // Charge les données d'un contrat 
        public static ContratModification? GetContratPourModification(int idContrat)
        {
            using var db = new EasyImmo0Context();

            var contrat = db.Contrats.FirstOrDefault(c => c.IdContrat == idContrat);
            if (contrat == null) return null;

            var bien = db.Biens.FirstOrDefault(b => b.IdContrat == idContrat);
            var effectuer = db.EffectuerOperationPersonnes
                .Where(e => e.IdContrat == idContrat).ToList();

            string typePersonne = (contrat.TypeContrat ?? "Vente").ToLower() == "vente"
                ? "Acheteur" : "Locataire";

            return new ContratModification
            {
                IdContrat              = idContrat,
                Description            = contrat.DescriptionOperation ?? string.Empty,
                TypeContrat            = contrat.TypeContrat ?? "Vente",
                DateOperation          = contrat.DateOperation,
                IdBien                 = bien?.IdBien,
                IdEmp                  = contrat.IdEmp,
                IdPersonneParticipant  = effectuer.FirstOrDefault(e => e.TypePersonne == typePersonne)?.IdPersonne,
                IdPersonneProprietaire = effectuer.FirstOrDefault(e => e.TypePersonne == "Propriétaire")?.IdPersonne
            };
        }

        // Modifie un contrat existant
        public static void ModifierContrat(
            int idContrat,
            string description,
            DateTime date,
            TypeContrat typeContrat,
            int? idEmp,
            int? idPersonneParticipant,
            int? idPersonneProprietaire)
        {
            try
            {
                using var db = new EasyImmo0Context();

                var contrat = db.Contrats.FirstOrDefault(c => c.IdContrat == idContrat);
                if (contrat == null)
                    throw new EntiteIntrouvableException("Ce contrat n'existe plus.");

                contrat.DescriptionOperation = description;
                contrat.DateOperation        = date;
                contrat.TypeContrat          = typeContrat.ToString();
                contrat.IdEmp                = idEmp;

                db.SaveChanges();
            }
            catch (EntiteIntrouvableException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible de modifier le contrat.", ex);
            }

            try
            {
                using var db = new EasyImmo0Context();

                // Mettre à jour les participants
                var anciens = db.EffectuerOperationPersonnes
                    .Where(e => e.IdContrat == idContrat).ToList();
                foreach (var a in anciens)
                    db.EffectuerOperationPersonnes.Remove(a);

                string typePersonne = typeContrat == TypeContrat.Vente ? "Acheteur" : "Locataire";

                if (idPersonneParticipant.HasValue)
                    db.EffectuerOperationPersonnes.Add(new EffectuerOperationPersonne
                    {
                        IdContrat  = idContrat,
                        IdPersonne = idPersonneParticipant.Value,
                        TypePersonne = typePersonne
                    });

                if (idPersonneProprietaire.HasValue)
                    db.EffectuerOperationPersonnes.Add(new EffectuerOperationPersonne
                    {
                        IdContrat  = idContrat,
                        IdPersonne = idPersonneProprietaire.Value,
                        TypePersonne = "Propriétaire"
                    });

                db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible de mettre à jour les participants.", ex);
            }
        }

        // Supprime un contrat et délie le bien
        public static void SupprimerContrat(int idContrat)
        {
            try
            {
                using var db = new EasyImmo0Context();

                var biens = db.Biens.Where(b => b.IdContrat == idContrat).ToList();
                foreach (var b in biens)
                    b.IdContrat = null;

                var participants = db.EffectuerOperationPersonnes
                    .Where(e => e.IdContrat == idContrat).ToList();
                foreach (var p in participants)
                    db.EffectuerOperationPersonnes.Remove(p);

                var contrat = db.Contrats.FirstOrDefault(c => c.IdContrat == idContrat);
                if (contrat == null)
                    throw new EntiteIntrouvableException("Ce contrat n'existe plus.");

                db.Contrats.Remove(contrat);
                db.SaveChanges();
            }
            catch (EntiteIntrouvableException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException("Impossible de supprimer le contrat.", ex);
            }
        }
    }

    // Détails complets d'un contrat pour la popup
    public class ContratDetail
    {
        public string Description { get; set; } = string.Empty;
        public string TypeContrat { get; set; } = string.Empty;
        public DateTime? DateContrat { get; set; }
        public string NomBien { get; set; } = "—";
        public string Adresse { get; set; } = "—";
        public decimal? PrixBien { get; set; }
        public string NomAgent { get; set; } = "—";
        public string LabelParticipant { get; set; } = "Acheteur";
        public string NomParticipantPrincipal { get; set; } = "—";
        public string NomProprietaire { get; set; } = "—";
    }

    // Données d'un contrat 
    public class ContratModification
    {
        public int IdContrat { get; set; }
        public string Description { get; set; } = string.Empty;
        public string TypeContrat { get; set; } = "Vente";
        public DateTime? DateOperation { get; set; }
        public int? IdBien { get; set; }
        public int? IdEmp { get; set; }
        public int? IdPersonneParticipant { get; set; }
        public int? IdPersonneProprietaire { get; set; }
    }

    // Classe d'affichage pour la liste des contrats
    public class ContratAffichage
    {
        public Contrat Contrat { get; set; } = null!;
        public int IdContrat => Contrat.IdContrat;
        public string Description => Contrat.DescriptionOperation ?? string.Empty;
        public DateTime? DateOperation => Contrat.DateOperation;
        public string TypeContrat => Contrat.TypeContrat ?? string.Empty;
        public string NomBien { get; set; } = string.Empty;
        public string NomAgent { get; set; } = "—";
        public string NomAcheteur { get; set; } = "—";
        public string NomLocataire { get; set; } = "—";
        public string NomProprietaire { get; set; } = "—";

        public string NomParticipant =>
            NomAcheteur != "—" ? NomAcheteur :
            NomLocataire != "—" ? NomLocataire : "—";
    }

    public class AgentInfo
    {
        public int IdEmp { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public override string ToString() => NomComplet;
    }
}
