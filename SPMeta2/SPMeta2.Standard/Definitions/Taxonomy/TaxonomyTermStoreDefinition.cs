using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    /// <summary>
    /// Allows to define and taxonomy store.
    /// Does not actualy deploy new taxonomy store, but used as 'model host' for other taxonomy related models - term roups, termsets, terms.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.TermStore", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermStore", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class TaxonomyTermStoreDefinition : DefinitionBase
    {
        #region properties

        public string Name { get; set; }
        public Guid? Id { get; set; }

        public bool? UseDefaultSiteCollectionTermStore { get; set; }

        #endregion
    }
}
