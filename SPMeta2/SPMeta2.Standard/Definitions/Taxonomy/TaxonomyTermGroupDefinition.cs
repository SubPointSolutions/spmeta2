using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and taxonomy term group.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.Group", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermGroup", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermStoreDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    public class TaxonomyTermGroupDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        public string Name { get; set; }

        [ExpectValidation]
        public Guid? Id { get; set; }

        [ExpectValidation]
        public bool IsSiteCollectionGroup { get; set; }

        //public bool IsSystemGroup { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TaxonomyTermGroupDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Id)
                          .ToString();
        }

        #endregion
    }
}
