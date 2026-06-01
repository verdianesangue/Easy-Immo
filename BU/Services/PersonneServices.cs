using BU;
using BU.Services;
using Common.Erreurs;
using Common.Exception;
using DAL.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BU.Services
{
    public class PersonneServices
    {

        /// <summary>
        /// Récupère tous les clients  avec leurs rôles et contacts
        /// </summary>
        public static List<ClientEntity> GetClients()
        {
            using (var db = new EasyImmo0Context())
            {
                var personnes = db.Personnes.ToList();
                var acheteurs = db.Acheteurs.ToList();
                var locataires = db.Locataires.ToList();
                var proprietaires = db.Proprietaires.ToList();
                var contacts = db.MoyenContacts.ToList();

                var clients = new List<ClientEntity>();

                foreach (var p in personnes)
                {
                    var acheteur = acheteurs.FirstOrDefault(a => a.IdPersonne == p.IdPersonne);
                    var locataire = locataires.FirstOrDefault(l => l.IdPersonne == p.IdPersonne);
                    var proprietaire = proprietaires.FirstOrDefault(pr => pr.IdPersonnes == p.IdPersonne);
                    var email = contacts.FirstOrDefault(c => c.IdPersonne == p.IdPersonne && c.TypeContact == "Email");
                    var tel = contacts.FirstOrDefault(c => c.IdPersonne == p.IdPersonne && c.TypeContact == "Telephone");

                    clients.Add(new ClientEntity
                    {
                        IdPersonne = p.IdPersonne,
                        Nom = p.NomPersonne ?? string.Empty,
                        Prenom = p.PrenomPersonne ?? string.Empty,
                        Email = email?.Valeur ?? string.Empty,
                        Telephone = tel?.Valeur ?? string.Empty,
                        EstAcheteur = acheteur != null,
                        EstLocataire = locataire != null,
                        EstProprietaire = proprietaire != null,
                        BudgetMaxAcheteur = acheteur?.BudgetMaxAch,
                        ZoneSouhaiteeAcheteur = acheteur?.ZoneSouhaiteAch,
                        MontantMaxLocataire = locataire?.MontantMax,
                        TypeProprietaire = Enum.TryParse<TypeProprietaire>(proprietaire?.TypePro, out var tp)
                            ? tp : TypeProprietaire.Particulier
                    });
                }

                return clients;
            }
        }

        public static int CompterClients()
        {
            using var db = new EasyImmo0Context();
            return db.Personnes.Count();
        }

        public static int CompterClientsParMois(DateTime date)
        {
            using var db = new EasyImmo0Context();
            return db.Acheteurs.Count(a => true) + db.Locataires.Count(l => true);
        }

        public static int CompterProprietaires()
        {
            using var db = new EasyImmo0Context();
            return db.Proprietaires.Count();
        }

        public static int CompterLocataires()
        {
            using var db = new EasyImmo0Context();
            return db.Locataires.Count();
        }

        public static int CompterAcheteurs()
        {
            using var db = new EasyImmo0Context();
            return db.Acheteurs.Count();
        }

        public static List<ClientEntity> GetListeLocataires()
            => GetClients().Where(c => c.EstLocataire).ToList();

        public static List<ClientEntity> GetListeAcheteurs()
            => GetClients().Where(c => c.EstAcheteur).ToList();

       
        // Recherche les clients par nom, prénom, email ou téléphone
        public static List<ClientEntity> RechercherClients(string motCle)
        {
            if (string.IsNullOrWhiteSpace(motCle))
                return GetClients();

            motCle = motCle.ToLower();

            return GetClients()
                .Where(c =>
                    c.Nom.ToLower().Contains(motCle) ||
                    c.Prenom.ToLower().Contains(motCle) ||
                    c.Email.ToLower().Contains(motCle) ||
                    c.Telephone.ToLower().Contains(motCle))
                .ToList();
        }


        // Récupère un client par son Id
        public static ClientEntity? GetClientById(int idPersonne)
        {
            return GetClients().FirstOrDefault(c => c.IdPersonne == idPersonne);
        }

        public static void AjouterClient(ClientEntity client)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    var personne = new Personne
                    {
                        NomPersonne = client.Nom,
                        PrenomPersonne = client.Prenom
                    };
                    db.Personnes.Add(personne);
                    db.SaveChanges();

                    int idPersonne = personne.IdPersonne;

                    // Ajouter l'email si existe
                    if (!string.IsNullOrWhiteSpace(client.Email))
                    {
                        db.MoyenContacts.Add(new MoyenContact
                        {
                            IdPersonne = idPersonne,
                            TypeContact = "Email",
                            Valeur = client.Email
                        });
                    }

                    // Ajouter le téléphone s'il existe
                    if (!string.IsNullOrWhiteSpace(client.Telephone))
                    {
                        db.MoyenContacts.Add(new MoyenContact
                        {
                            IdPersonne = idPersonne,
                            TypeContact = "Telephone",
                            Valeur = client.Telephone
                        });
                    }

                    // Ajouter les roles cochés
                    if (client.EstAcheteur)
                    {
                        db.Acheteurs.Add(new Acheteur
                        {
                            IdPersonne = idPersonne,
                            BudgetMaxAch = client.BudgetMaxAcheteur,
                            ZoneSouhaiteAch = client.ZoneSouhaiteeAcheteur
                        });
                    }

                    if (client.EstLocataire)
                    {
                        db.Locataires.Add(new Locataire
                        {
                            IdPersonne = idPersonne,
                            MontantMax = client.MontantMaxLocataire
                        });
                    }

                    if (client.EstProprietaire)
                    {
                        db.Proprietaires.Add(new Proprietaire
                        {
                            IdPersonnes = idPersonne,
                            TypePro = client.TypeProprietaire.ToString()
                        });
                    }

                    db.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible d'enregistrer le client en base de données", ex);
            }
        }

      

        public static void ModifierClient(ClientEntity client)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    
                    var personne = db.Personnes.FirstOrDefault(p => p.IdPersonne == client.IdPersonne);
                    if (personne == null)
                    {
                        throw new EntiteIntrouvableException(
                            "Ce client n'existe plus. Il a peut-être été supprimé");
                    }

                    personne.NomPersonne = client.Nom;
                    personne.PrenomPersonne = client.Prenom;

                   
                    var emailExistant = db.MoyenContacts.FirstOrDefault(c => c.IdPersonne == client.IdPersonne && c.TypeContact == "Email");
                    if (emailExistant != null)
                        emailExistant.Valeur = client.Email;
                    else if (!string.IsNullOrWhiteSpace(client.Email))
                        db.MoyenContacts.Add(new MoyenContact { IdPersonne = client.IdPersonne, TypeContact = "Email", Valeur = client.Email });

                    // Mettre à jour ou créer le téléphone
                    var telExistant = db.MoyenContacts.FirstOrDefault(c => c.IdPersonne == client.IdPersonne && c.TypeContact == "Telephone");
                    if (telExistant != null)
                        telExistant.Valeur = client.Telephone;
                    else if (!string.IsNullOrWhiteSpace(client.Telephone))
                        db.MoyenContacts.Add(new MoyenContact { IdPersonne = client.IdPersonne, TypeContact = "Telephone", Valeur = client.Telephone });

                    // Gérer le rôle Acheteur
                    var acheteur = db.Acheteurs.FirstOrDefault(a => a.IdPersonne == client.IdPersonne);
                    if (client.EstAcheteur)
                    {
                        if (acheteur == null)
                        {
                            db.Acheteurs.Add(new Acheteur
                            {
                                IdPersonne = client.IdPersonne,
                                BudgetMaxAch = client.BudgetMaxAcheteur,
                                ZoneSouhaiteAch = client.ZoneSouhaiteeAcheteur
                            });
                        }
                        else
                        {
                            acheteur.BudgetMaxAch = client.BudgetMaxAcheteur;
                            acheteur.ZoneSouhaiteAch = client.ZoneSouhaiteeAcheteur;
                        }
                    }
                    else if (acheteur != null)
                    {
                        db.Acheteurs.Remove(acheteur);
                    }

                    // Gérer le rôle Locataire
                    var locataire = db.Locataires.FirstOrDefault(l => l.IdPersonne == client.IdPersonne);
                    if (client.EstLocataire)
                    {
                        if (locataire == null)
                        {
                            db.Locataires.Add(new Locataire
                            {
                                IdPersonne = client.IdPersonne,
                                MontantMax = client.MontantMaxLocataire
                            });
                        }
                        else
                        {
                            locataire.MontantMax = client.MontantMaxLocataire;
                        }
                    }
                    else if (locataire != null)
                    {
                        db.Locataires.Remove(locataire);
                    }

                    //  Gérer le rôle Propriétaire
                    var proprietaire = db.Proprietaires.FirstOrDefault(p => p.IdPersonnes == client.IdPersonne);
                    if (client.EstProprietaire)
                    {
                        if (proprietaire == null)
                        {
                            db.Proprietaires.Add(new Proprietaire
                            {
                                IdPersonnes = client.IdPersonne,
                                TypePro = client.TypeProprietaire.ToString()
                            });
                        }
                        else
                        {
                            proprietaire.TypePro = client.TypeProprietaire.ToString();
                        }
                    }
                    else if (proprietaire != null)
                    {
                        db.Proprietaires.Remove(proprietaire);
                    }

                    db.SaveChanges();
                   
                }
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible de modifier le client en base de données.", ex);
            }
        }



        public static void SupprimerClient(int idPersonne)
        {
            try
            {
                using (var db = new EasyImmo0Context())
                {
                    // Vérifier que la personne existe 
                    var personne = db.Personnes.FirstOrDefault(p => p.IdPersonne == idPersonne);
                    if (personne == null)
                    {
                        throw new EntiteIntrouvableException("Ce client n'existe plus.");
                    }

                    string nomComplet = $"{personne.PrenomPersonne} {personne.NomPersonne}";

                    
                    bool aDesOperations = db.EffectuerOperationPersonnes
                        .Any(e => e.IdPersonne == idPersonne);
                    if (aDesOperations)
                    {
                        throw new SuppressionImpossibleException(
                            $"Impossible de supprimer {nomComplet} : ce client est lié à une ou " +
                            "plusieurs opérations immobilières. Veuillez d'abord traiter ces opérations.");
                    }

                    
                    bool aDesActivites = db.ParticiperActivitePersonnes
                        .Any(p => p.IdPersonne == idPersonne);
                    if (aDesActivites)
                    {
                        throw new SuppressionImpossibleException(
                            $"Impossible de supprimer {nomComplet} : ce client participe à une ou " +
                            "plusieurs activités. Veuillez d'abord traiter ces activités.");
                    }

                    
                    bool estEmploye = db.Employes
                        .Any(e => e.IdPersonne == idPersonne);
                    if (estEmploye)
                    {
                        throw new SuppressionImpossibleException(
                            $"Impossible de supprimer {nomComplet} : cette personne est aussi un " +
                            "employé de l'agence. Veuillez d'abord gérer son dossier employé.");
                    }

                    
                    var proprietaire = db.Proprietaires
                        .FirstOrDefault(p => p.IdPersonnes == idPersonne);
                    if (proprietaire != null)
                    {
                        bool possedeDesBiens = db.PossederBienProprietaires
                            .Any(pb => pb.IdPro == proprietaire.IdPro);
                        if (possedeDesBiens)
                        {
                            throw new SuppressionImpossibleException(
                                $"Impossible de supprimer {nomComplet} : ce client possède un ou " +
                                "plusieurs biens. Veuillez d'abord réaffecter ces biens.");
                        }
                    }

                    
                    var acheteur = db.Acheteurs.FirstOrDefault(a => a.IdPersonne == idPersonne);
                    if (acheteur != null) db.Acheteurs.Remove(acheteur);

                    var locataire = db.Locataires.FirstOrDefault(l => l.IdPersonne == idPersonne);
                    if (locataire != null) db.Locataires.Remove(locataire);

                    if (proprietaire != null) db.Proprietaires.Remove(proprietaire);

                    // Supprimer les contacts 
                    var contacts = db.MoyenContacts.Where(c => c.IdPersonne == idPersonne).ToList();
                    foreach (var c in contacts)
                        db.MoyenContacts.Remove(c);

                    
                    db.Personnes.Remove(personne);

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
                    "Une erreur technique est survenue lors de la suppression.", ex);
            }
        }

        public static List<EmployeEntity> GetEmployes()
        {
            using var db = new EasyImmo0Context();

            return db.Employes
                .Include(e => e.IdEmpNavigation)
                    .ThenInclude(p => p.MoyenContacts)
                .ToList()
                .Select(e =>
                {
                    var p     = e.IdEmpNavigation;
                    var email = p?.MoyenContacts.FirstOrDefault(c => c.TypeContact == "Email");
                    var tel   = p?.MoyenContacts.FirstOrDefault(c => c.TypeContact == "Telephone");

                    return new EmployeEntity
                    {
                        IdEmp      = e.IdEmp,
                        IdPersonne = e.IdPersonne,
                        Nom        = p?.NomPersonne    ?? string.Empty,
                        Prenom     = p?.PrenomPersonne ?? string.Empty,
                        Email      = email?.Valeur     ?? string.Empty,
                        Telephone  = tel?.Valeur       ?? string.Empty,
                        Role       = e.RoleEmp         ?? string.Empty
                    };
                }).ToList();
        }

        public static EmployeEntity? GetEmployeById(int idEmp)
            => GetEmployes().FirstOrDefault(e => e.IdEmp == idEmp);

        public static List<EmployeEntity> RechercherEmployes(string motCle)
        {
            if (string.IsNullOrWhiteSpace(motCle))
                return GetEmployes();

            motCle = motCle.ToLower();
            return GetEmployes().Where(e =>
                e.Nom.ToLower().Contains(motCle)    ||
                e.Prenom.ToLower().Contains(motCle) ||
                e.Email.ToLower().Contains(motCle)  ||
                e.Role.ToLower().Contains(motCle))
                .ToList();
        }

        public static void AjouterEmploye(EmployeEntity employe)
        {
            try
            {
                using var db = new EasyImmo0Context();

                var personne = new Personne
                {
                    NomPersonne    = employe.Nom,
                    PrenomPersonne = employe.Prenom
                };
                db.Personnes.Add(personne);
                db.SaveChanges();

                int idPersonne = personne.IdPersonne;

                if (!string.IsNullOrWhiteSpace(employe.Email))
                    db.MoyenContacts.Add(new MoyenContact
                    {
                        IdPersonne  = idPersonne,
                        TypeContact = "Email",
                        Valeur      = employe.Email
                    });

                if (!string.IsNullOrWhiteSpace(employe.Telephone))
                    db.MoyenContacts.Add(new MoyenContact
                    {
                        IdPersonne  = idPersonne,
                        TypeContact = "Telephone",
                        Valeur      = employe.Telephone
                    });

                db.Employes.Add(new Employe
                {
                    IdPersonne = idPersonne,
                    RoleEmp    = employe.Role
                });

                db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible d'enregistrer l'employé en base de données.", ex);
            }
        }

        public static void ModifierEmploye(EmployeEntity employe)
        {
            try
            {
                using var db = new EasyImmo0Context();

                var emp = db.Employes.FirstOrDefault(e => e.IdEmp == employe.IdEmp);
                if (emp == null)
                    throw new EntiteIntrouvableException("Cet employé n'existe plus.");

                var personne = db.Personnes.FirstOrDefault(p => p.IdPersonne == emp.IdPersonne);
                if (personne != null)
                {
                    personne.NomPersonne    = employe.Nom;
                    personne.PrenomPersonne = employe.Prenom;
                }

                emp.RoleEmp = employe.Role;

                var emailExistant = db.MoyenContacts.FirstOrDefault(
                    c => c.IdPersonne == emp.IdPersonne && c.TypeContact == "Email");
                if (emailExistant != null)
                    emailExistant.Valeur = employe.Email;
                else if (!string.IsNullOrWhiteSpace(employe.Email))
                    db.MoyenContacts.Add(new MoyenContact
                    {
                        IdPersonne  = emp.IdPersonne,
                        TypeContact = "Email",
                        Valeur      = employe.Email
                    });

                var telExistant = db.MoyenContacts.FirstOrDefault(
                    c => c.IdPersonne == emp.IdPersonne && c.TypeContact == "Telephone");
                if (telExistant != null)
                    telExistant.Valeur = employe.Telephone;
                else if (!string.IsNullOrWhiteSpace(employe.Telephone))
                    db.MoyenContacts.Add(new MoyenContact
                    {
                        IdPersonne  = emp.IdPersonne,
                        TypeContact = "Telephone",
                        Valeur      = employe.Telephone
                    });

                db.SaveChanges();
            }
            catch (EntiteIntrouvableException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Impossible de modifier l'employé en base de données.", ex);
            }
        }

        public static void SupprimerEmploye(int idEmp)
        {
            try
            {
                using var db = new EasyImmo0Context();

                var emp = db.Employes.FirstOrDefault(e => e.IdEmp == idEmp);
                if (emp == null)
                    throw new EntiteIntrouvableException("Cet employé n'existe plus.");

                bool aDesContrats = db.Contrats.Any(c => c.IdEmp == idEmp);
                if (aDesContrats)
                    throw new SuppressionImpossibleException(
                        "Impossible de supprimer cet employé : il est lié à un ou plusieurs contrats.");

                var contacts = db.MoyenContacts
                    .Where(c => c.IdPersonne == emp.IdPersonne).ToList();
                foreach (var c in contacts)
                    db.MoyenContacts.Remove(c);

                var personne = db.Personnes.FirstOrDefault(p => p.IdPersonne == emp.IdPersonne);

                db.Employes.Remove(emp);

                if (personne != null)
                    db.Personnes.Remove(personne);

                db.SaveChanges();
            }
            catch (EntiteIntrouvableException)     { throw; }
            catch (SuppressionImpossibleException) { throw; }
            catch (System.Exception ex)
            {
                throw new ErreurBaseDeDonneesException(
                    "Une erreur technique est survenue lors de la suppression.", ex);
            }
        }

        public static ResultatValidation ValiderEmploye(EmployeEntity employe)
        {
            var resultat = new ResultatValidation();

            if (string.IsNullOrWhiteSpace(employe.Nom))
                resultat.AjouterErreur("Le nom est obligatoire.");
            else if (employe.Nom.Length > 50)
                resultat.AjouterErreur("Le nom ne peut pas dépasser 50 caractères.");

            if (string.IsNullOrWhiteSpace(employe.Prenom))
                resultat.AjouterErreur("Le prénom est obligatoire.");
            else if (employe.Prenom.Length > 50)
                resultat.AjouterErreur("Le prénom ne peut pas dépasser 50 caractères.");

            if (string.IsNullOrWhiteSpace(employe.Role))
                resultat.AjouterErreur("Le rôle est obligatoire.");

            if (!string.IsNullOrWhiteSpace(employe.Email))
                if (!employe.Email.Contains("@") || !employe.Email.Contains("."))
                    resultat.AjouterErreur("L'email n'est pas valide.");

            return resultat;
        }

        // Verifie que les donnees du client sont valides
        public static ResultatValidation ValiderClient(ClientEntity client)
        {
            var resultat = new ResultatValidation();

            
            if (string.IsNullOrWhiteSpace(client.Nom))
            {
                resultat.AjouterErreur("Le nom est obligatoire");
            }
            else if (client.Nom.Length > 50)
            {
                resultat.AjouterErreur("Le nom ne peut pas dépasser 50 caractères");
            }

            
            if (string.IsNullOrWhiteSpace(client.Prenom))
            {
                resultat.AjouterErreur("Le prénom est obligatoire.");
            }
            else if (client.Prenom.Length > 50)
            {
                resultat.AjouterErreur("Le prénom ne peut pas dépasser 50 caractères");
            }

            
            if (!string.IsNullOrWhiteSpace(client.Email))
            {
                if (!client.Email.Contains("@") || !client.Email.Contains("."))
                {
                    resultat.AjouterErreur("L'email n'est pas valide (exemple : verdiane@gmail.com)");
                }
            }

            
            if (!string.IsNullOrWhiteSpace(client.Telephone))
            {
                // On compte le nombre de chiffres dans le téléphone en ignorant les espaces, tirets
                var chiffres = client.Telephone.Where(c => char.IsDigit(c)).Count();
                if (chiffres < 8)
                {
                    resultat.AjouterErreur("Le téléphone doit contenir au moins 8 chiffres");
                }
            }

            // Au moins un rôle doit être coché 
            if (!client.EstAcheteur && !client.EstLocataire && !client.EstProprietaire)
            {
                resultat.AjouterErreur("Veuillez sélectionner au moins un rôle (Acheteur, Locataire ou Propriétaire)");
            }

            
            if (client.EstAcheteur)
            {
                if (client.BudgetMaxAcheteur.HasValue && client.BudgetMaxAcheteur.Value <= 0)
                {
                    resultat.AjouterErreur("Le budget max de l'acheteur doit être positif");
                }
            }

           
            if (client.EstLocataire)
            {
                if (client.MontantMaxLocataire.HasValue && client.MontantMaxLocataire.Value <= 0)
                {
                    resultat.AjouterErreur("Le montant max du locataire doit être positif");
                }
            }


            return resultat;
        }
    }

    public class EmployeEntity
    {
        public int IdEmp { get; set; }
        public int IdPersonne { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string NomComplet => $"{Prenom} {Nom}";
    }
}