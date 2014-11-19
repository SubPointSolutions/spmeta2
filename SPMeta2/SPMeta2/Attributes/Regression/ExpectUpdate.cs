using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing infrastructure to indicate properties which have to be changes with a new provision.
    /// </summary>
    public class ExpectUpdate : Attribute
    {
    }
}
