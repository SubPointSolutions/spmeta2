using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

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
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]


    [ParentHostCapability(typeof(TaxonomyTermDefinition))]
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
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public int LCID { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool IsDefault { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("LCID", LCID)
                          .AddRawPropertyValue("IsDefault", IsDefault)
                          .ToString();
        }

        #endregion
    }
}
