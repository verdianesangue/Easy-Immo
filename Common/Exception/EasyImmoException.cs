using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exception
{
    /// <summary>
    /// Classe parent de toutes les VRAIES exceptions techniques de l'app.
    /// Permet de toutes les attraper en une fois si besoin.
    /// </summary>
    public class EasyImmoException : System.Exception
    {
        public EasyImmoException(string message) : base(message) { }
        public EasyImmoException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
