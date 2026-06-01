using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exception
{
    /// <summary>
    /// Exception levée quand il y a un probleme de connexion ou autre avec la DB
    /// </summary>
    public class ErreurBaseDeDonneesException : EasyImmoException
    {
        public ErreurBaseDeDonneesException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
