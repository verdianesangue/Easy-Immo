using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exception
{
    /// <summary>
    /// Exception levée quand on essaie de manipuler une entité qui n'existe plus
    /// </summary>
    public class EntiteIntrouvableException : EasyImmoException
    {
        public EntiteIntrouvableException(string message) : base(message) { }
    }
}
