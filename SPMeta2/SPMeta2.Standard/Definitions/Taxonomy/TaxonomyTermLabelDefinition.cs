using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and deploy taxonomy term.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.Label", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.Label", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    public class TaxonomyTermLabelDefinition : DefinitionBase
    {
        #region constructors

        public TaxonomyTermLabelDefinition()
        {
            LCID = 1033;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public string Name { get; set; }

        [ExpectValidation]
        public int LCID { get; set; }

        [ExpectValidation]
        public bool IsDefault { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TaxonomyTermLabelDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.LCID)
                          .AddPropertyValue(p => p.IsDefault)
                          .ToString();
        }

        #endregion
    }
}
