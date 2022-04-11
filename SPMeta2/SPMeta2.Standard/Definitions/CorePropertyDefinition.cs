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

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy user profile property.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.UserProfiles.CoreProperty", "Microsoft.Office.Server.UserProfiles")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

    public class CorePropertyDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired]
        public string DisplayName { get; set; }

        [ExpectValidation]
        [DataMember]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired]
        public string Type { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IsAlias { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IsMultivalued { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IsSearchable { get; set; }

        [ExpectValidation]
        [DataMember]
        public int? Length { get; set; }



        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("DisplayName", DisplayName)
                          .ToString();
        }

        #endregion
    }
}
