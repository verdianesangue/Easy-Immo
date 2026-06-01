using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BU
{
  
   

    public enum TypeProprietaire
    {
        Particulier,
        Societe
    }
    public enum TypeContrat
    {
        Vente,
        Location
    }

    public enum TypeActiviteEnum
    {
        Visite = 1,
        AppelTelephonique = 2,
        EnvoiEmail = 3,
        SignatureContrat = 4,
        AnalyseDossier = 5,
        RendezVousAgence = 6,
        PublicationAnnonce = 7,
        MiseAJourDossier = 8,
        RelanceClient = 9,
        EvaluationDuBien = 10,
        DemandeInfos = 12,
        SignatureCompromis = 13
    }
}
