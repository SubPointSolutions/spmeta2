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
    /// Allows to define and taxonomy term group.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.Group", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermGroup", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermStoreDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(TaxonomyTermStoreDefinition))]

    [ExpectManyInstances]

    public class TaxonomyTermGroupDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public Guid? Id { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public bool IsSiteCollectionGroup { get; set; }

        //public bool IsSystemGroup { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Id", Id)
                          .ToString();
        }

        #endregion
    }
}
