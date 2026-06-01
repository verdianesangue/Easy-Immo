using BU;
using System.Collections.Generic;

namespace BU.Services
{
    public class ClientEntity
    {
        public int IdPersonne { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;

        public bool EstAcheteur { get; set; }
        public bool EstLocataire { get; set; }
        public bool EstProprietaire { get; set; }

        public decimal? BudgetMaxAcheteur { get; set; }
        public string? ZoneSouhaiteeAcheteur { get; set; }
        public decimal? MontantMaxLocataire { get; set; }
        public TypeProprietaire TypeProprietaire { get; set; } = TypeProprietaire.Particulier;

        public string NomComplet => $"{Prenom} {Nom}";

        public string RolesAffichage
        {
            get
            {
                var roles = new List<string>();
                if (EstAcheteur) roles.Add("Acheteur");
                if (EstLocataire) roles.Add("Locataire");
                if (EstProprietaire) roles.Add("Propriétaire");
                return roles.Count > 0 ? string.Join(", ", roles) : "Aucun rôle";
            }
        }
    }
}
