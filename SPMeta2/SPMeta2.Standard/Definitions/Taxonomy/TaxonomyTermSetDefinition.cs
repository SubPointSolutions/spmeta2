using System;
using System.Collections.Generic;
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
    /// Allows to define and taxonomy termset.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.TermSet", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TermSet", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(TaxonomyTermGroupDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(TaxonomyTermGroupDefinition))]

    [ExpectManyInstances]

    public class TaxonomyTermSetDefinition : DefinitionBase
    {
        #region constructors

        public TaxonomyTermSetDefinition()
        {
            LCID = 1033;

            CustomProperties = new List<TaxonomyTermCustomProperty>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired(GroupName = "Term Identity")]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Description { get; set; }

        [ExpectValidation]
        [ExpectRequired(GroupName = "Term Identity")]
        [DataMember]
        [IdentityKey]
        public Guid? Id { get; set; }

        [ExpectValidation]
        [DataMember]
        public int LCID { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? IsAvailableForTagging { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? IsOpenForTermCreation { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string CustomSortOrder { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [ExpectNullable]
        [DataMember]
        public string Contact { get; set; }



        [ExpectValidation]
        [DataMember]
        public List<TaxonomyTermCustomProperty> CustomProperties { get; set; }


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
