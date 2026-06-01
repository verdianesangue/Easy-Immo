using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Common.Erreurs
{
    /// <summary>
    /// Représente le résultat d'une validation
    /// </summary>
    public class ResultatValidation
    {
        public bool EstValide { get; private set; } = true;
        public List<string> Erreurs { get; } = new List<string>();

        
        public void AjouterErreur(string erreur)
        {
            Erreurs.Add(erreur);
            EstValide = false;
        }

        // Affiche les erreurs sous forme de message 
        public string ToMessage()
        {
            return string.Join("\n", Erreurs);
        }
    }
}