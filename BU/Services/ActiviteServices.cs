using BU;
using Common.Erreurs;
using Common.Exception;
using DAL.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BU.Services
{
    public class ActiviteServices
    {

        

        /// <summary>
        /// Récupère toutes les activités
        /// </summary>
        public static List<Activite> GetActivites()
        {
            using var db = new EasyImmo0Context();
            return db.Activites
                .Include(a => a.IdBienNavigation)
                .Include(a => a.IdTypeNavigation)
                .ToList();
        }


        // Compter toutes les activités
        public static int CompterActivites()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Activites.Count();
            }
        }


        // Recherche les activités par libellé
        public static List<Activite> RechercherActivites(string motCle)
        {
            if (string.IsNullOrWhiteSpace(motCle))
                return GetActivites();

            motCle = motCle.ToLower();

            return GetActivites()
                .Where(a =>
                    (a.LibelleActivite != null && a.LibelleActivite.ToLower().Contains(motCle)))
                .ToList();
        }


        // Récupère une activité précise par son Id
        public static Activite? GetActiviteById(int idActivite)
        {
            using var db = new EasyImmo0Context();
            return db.Activites
                .Include(a => a.IdBienNavigation)
                .Include(a => a.IdTypeNavigation)
                .FirstOrDefault(a => a.IdActivite == idActivite);
        }


        // Récupère la liste de tous les biens 
        public static List<Bien> GetAllBiens()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Biens.ToList();
            }
        }


        // Récupère le nom d'un bien par son id 
        public static string GetNomBien(int idBien)
        {
            using (var db = new EasyImmo0Context())
            {
                var bien = db.Biens.FirstOrDefault(b => b.IdBien == idBien);
                return bien?.NomBien ?? "Bien inconnu";
            }
        }


        public static void AjouterActivite(Activite activite)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    db.Activites.Add(activite);
                    db.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible d'enregistrer l'activité en base de données", ex);
            }
        }


        public static void ModifierActivite(Activite activite)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {

                    var existante = db.Activites.FirstOrDefault(a => a.IdActivite == activite.IdActivite);
                    if (existante == null)
                    {
                        throw new EntiteIntrouvableException(
                            "Cette activité n'existe plus. Elle a peut-être été supprimée");
                    }

                    existante.LibelleActivite = activite.LibelleActivite;
                    existante.DateActivite = activite.DateActivite;
                    existante.DureeActivite = activite.DureeActivite;
                    existante.IdBien = activite.IdBien;
                    existante.IdType = activite.IdType;

                    db.SaveChanges();

                }
            }
            catch (EntiteIntrouvableException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible de modifier l'activité en base de données.", ex);
            }
        }


        public static void SupprimerActivite(int idActivite)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var activite = db.Activites.FirstOrDefault(a => a.IdActivite == idActivite);
                    if (activite == null)
                    {
                        throw new EntiteIntrouvableException("Cette activité n'existe plus.");
                    }

                    // Supprimer d'abord les participants 
                    var participants = db.ParticiperActivitePersonnes
                        .Where(p => p.IdActivite == idActivite).ToList();
                    foreach (var p in participants)
                        db.ParticiperActivitePersonnes.Remove(p);

                    db.Activites.Remove(activite);
                    db.SaveChanges();
                }
            }
            catch (EntiteIntrouvableException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Une erreur technique est survenue lors de la suppression.", ex);
            }
        }


        // Pour la gestion des erreurs, vérifie que les données de l'activité sont valides 
        public static ResultatValidation ValiderActivite(Activite activite)
        {
            var resultat = new ResultatValidation();

            if (string.IsNullOrWhiteSpace(activite.LibelleActivite))
            {
                resultat.AjouterErreur("Le libellé de l'activité est obligatoire");
            }
            else if (activite.LibelleActivite.Length > 50)
            {
                resultat.AjouterErreur("Le libellé ne peut pas dépasser 50 caractères");
            }

            if (!activite.DateActivite.HasValue)
            {
                resultat.AjouterErreur("La date de l'activité est obligatoire");
            }

            if (activite.IdBien <= 0)
            {
                resultat.AjouterErreur("Veuillez sélectionner un bien");
            }

            if (activite.IdType <= 0)
            {
                resultat.AjouterErreur("Veuillez sélectionner un type d'activité");
            }

            return resultat;
        }


        

        /// <summary>
        /// Récupère les personnes qui participent à une activité
        /// </summary>
        public static List<Personne> GetParticipants(int idActivite)
        {
            using (var db = new EasyImmo0Context())
            {
                var idsPersonnes = db.ParticiperActivitePersonnes
                    .Where(p => p.IdActivite == idActivite)
                    .Select(p => p.IdPersonne)
                    .ToList();

                return db.Personnes
                    .Where(per => idsPersonnes.Contains(per.IdPersonne))
                    .ToList();
            }
        }


      
        /// <summary>
        /// Ajoute une personne comme participant à une activité.
        /// Vérifie d'abord qu'elle n'est pas déjà occupée à ce moment-là.
        /// </summary>
        public static void AjouterParticipant(int idActivite, int idPersonne, string but = "Participant")
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    
                    var dejaParticipant = db.ParticiperActivitePersonnes
                        .Any(p => p.IdActivite == idActivite && p.IdPersonne == idPersonne);

                    if (dejaParticipant)
                    {
                        throw new SuppressionImpossibleException(
                            "Cette personne participe déjà à cette activité.");
                    }

                    //  Récupérer l'activité qu'on veut rejoindre 
                    var activiteCible = db.Activites.FirstOrDefault(a => a.IdActivite == idActivite);
                    if (activiteCible == null)
                    {
                        throw new EntiteIntrouvableException("Cette activité n'existe plus.");
                    }

                    //  Vérifier le conflit d'horaire 
                    if (activiteCible.DateActivite.HasValue)
                    {
                        
                        DateTime debutCible = activiteCible.DateActivite.Value;
                        DateTime finCible = CalculerFin(debutCible, activiteCible.DureeActivite);


                        var idsActivitesPersonne = db.ParticiperActivitePersonnes
                            .Where(p => p.IdPersonne == idPersonne)
                            .Select(p => p.IdActivite)
                            .ToList();

                        var autresActivites = db.Activites
                            .Where(a => idsActivitesPersonne.Contains(a.IdActivite) && a.DateActivite.HasValue)
                            .ToList();

                       
                        foreach (var autre in autresActivites)
                        {
                            DateTime debutAutre = autre.DateActivite!.Value;
                            DateTime finAutre = CalculerFin(debutAutre, autre.DureeActivite);

                            bool chevauchement = debutCible < finAutre && debutAutre < finCible;

                            if (chevauchement)
                            {
                                throw new SuppressionImpossibleException(
                                    $"Cette personne est déjà occupée le " +
                                    $"{debutAutre:dd/MM/yyyy à HH:mm} avec l'activité \"{autre.LibelleActivite}\". " +
                                    "Impossible de l'inscrire à deux activités en même temps.");
                            }
                        }
                    }

                    db.ParticiperActivitePersonnes.Add(new ParticiperActivitePersonne
                    {
                        IdActivite = idActivite,
                        IdPersonne = idPersonne,
                        But = but
                    });

                    db.SaveChanges();
                }
            }
            catch (EntiteIntrouvableException)
            {
                throw;
            }
            catch (SuppressionImpossibleException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible d'ajouter le participant à l'activité.", ex);
            }
        }

        /// <summary>
        /// Calcule l'heure de fin d'une activité à partir de son début et sa durée.
        /// </summary>
        private static DateTime CalculerFin(DateTime debut, TimeOnly? duree)
        {
            if (duree.HasValue)
            {
                return debut.AddHours(duree.Value.Hour).AddMinutes(duree.Value.Minute);
            }
            return debut.AddHours(1);
        }

        /// <summary>
        /// Retire une personne d'une activité
        /// </summary>
        public static void RetirerParticipant(int idActivite, int idPersonne)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var participation = db.ParticiperActivitePersonnes
                        .FirstOrDefault(p => p.IdActivite == idActivite && p.IdPersonne == idPersonne);

                    if (participation == null)
                    {
                        throw new EntiteIntrouvableException(
                            "Cette personne ne participe pas à cette activité.");
                    }

                    db.ParticiperActivitePersonnes.Remove(participation);
                    db.SaveChanges();
                }
            }
            catch (EntiteIntrouvableException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible de retirer le participant.", ex);
            }
        }


        /// <summary>
        /// Récupère toutes les personnes 
        /// </summary>
        public static List<Personne> GetAllPersonnes()
        {
            using (var db = new EasyImmo0Context())
            {
                return db.Personnes.ToList();
            }
        }

        public static int CompterVisites()
        {
            using var db = new EasyImmo0Context();
            return db.Activites.Count(a => a.IdType == 1);
        }

        public static int CompterVisitesParMois(DateTime date)
        {
            using var db = new EasyImmo0Context();
            return db.Activites.Count(a => a.IdType == 1
                && a.DateActivite.HasValue
                && a.DateActivite.Value.Month == date.Month
                && a.DateActivite.Value.Year == date.Year);
        }
    }

    public class ActiviteInfo
    {
        public string LibelleActivite { get; set; } = string.Empty;
        public DateTime? DateActivite { get; set; }
        public string NomType { get; set; } = string.Empty;
        public string Participants { get; set; } = string.Empty;
    }
}