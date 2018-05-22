using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;
using System.Collections.Generic;

namespace SPMeta2.Standard.Definitions.Taxonomy
{
    [Serializable]
    [DataContract]
    public class TaxonomyTermCustomProperty
    {
        public TaxonomyTermCustomProperty()
        {
            Override = true;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public bool Override { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy taxonomy term.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.Term", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.Term", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermSetDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]


    [ParentHostCapability(typeof(TaxonomyTermSetDefinition))]
    [ParentHostCapability(typeof(TaxonomyTermDefinition))]

    [ExpectManyInstances]


    public class TaxonomyTermDefinition : DefinitionBase
    {
        #region constructors

        public TaxonomyTermDefinition()
        {
            LCID = 1033;

            CustomProperties = new List<TaxonomyTermCustomProperty>();
            LocalCustomProperties = new List<TaxonomyTermCustomProperty>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired]
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
        public int LCID { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<TaxonomyTermCustomProperty> CustomProperties { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<TaxonomyTermCustomProperty> LocalCustomProperties { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string CustomSortOrder { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public bool? IsAvailableForTagging { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Id", Id)
                          .AddRawPropertyValue("LCID", LCID)
                          .ToString();
        }

        #endregion
    }
}
