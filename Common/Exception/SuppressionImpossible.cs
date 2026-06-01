using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exception
{
    /// <summary>
    /// Exception levée quand on ne peut pas supprimer directementune entité
    /// </summary>
    public class SuppressionImpossibleException : EasyImmoException
    {
        public SuppressionImpossibleException(string message) : base(message) { }
    }
}
