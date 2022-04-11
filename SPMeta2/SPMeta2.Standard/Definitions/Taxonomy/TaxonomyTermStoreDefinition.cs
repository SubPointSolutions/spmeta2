using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

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
    [DataContract]
    [ExpectAddHostExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    public class TaxonomyTermStoreDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public Guid? Id { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool? UseDefaultSiteCollectionTermStore { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Id", Id)
                          .AddRawPropertyValue("UseDefaultSiteCollectionTermStore", UseDefaultSiteCollectionTermStore)
                          .ToString();
        }

        #endregion
    }
}
