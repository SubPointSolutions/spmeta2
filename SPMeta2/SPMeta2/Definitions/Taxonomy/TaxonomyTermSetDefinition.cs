using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and taxonomy termset.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.TermSet", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermSet", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermGroupDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
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
