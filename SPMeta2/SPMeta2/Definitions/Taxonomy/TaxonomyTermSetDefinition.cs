using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions.Taxonomy
{
    public class TaxonomyTermSetDefinition : DefinitionBase
    {
         #region constructors

        public TaxonomyTermSetDefinition()
        {
            LCID = 1033;
        }

        #endregion

        #region properties

        public string Name { get; set; }
        public Guid? Id { get; set; }

        public int LCID { get; set; }

        #endregion
    }
}
