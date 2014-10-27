using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and deploy taxonomy term.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.Term", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.Term", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermSetDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class TaxonomyTermDefinition : DefinitionBase
    {
        #region constructors

        public TaxonomyTermDefinition()
        {
            LCID = 1033;
        }

        #endregion

        #region properties

        public string Name { get; set; }
        public Guid? Id { get; set; }

        public int LCID { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TaxonomyTermDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.LCID)
                          .ToString();
        }

        #endregion
    }
}
