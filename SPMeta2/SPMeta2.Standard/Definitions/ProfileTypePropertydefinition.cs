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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.UserProfiles.ProfileTypeProperty", "Microsoft.Office.Server.UserProfiles")]

    [DefaultParentHost(typeof(CorePropertyDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(CorePropertyDefinition))]

    //[ExpectManyInstances]
    [SingletonIdentity]

    public class ProfileTypePropertyDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool IsReplicable { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool IsVisibleOnEditor { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool IsVisibleOnViewer { get; set; }

        [ExpectValidation]
        [DataMember]
        public int? MaximumShown { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("IsVisibleOnEditor", IsVisibleOnEditor)
                          .AddRawPropertyValue("IsVisibleOnViewer", IsVisibleOnViewer)
                          .ToString();
        }

        #endregion
    }
}
