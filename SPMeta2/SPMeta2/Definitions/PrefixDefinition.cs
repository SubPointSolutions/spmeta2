using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint managed path.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPPrefix", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebApplicationDefinition))]

    [ExpectManyInstances]

    public class PrefixDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Path of the target managed path.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Path { get; set; }

        /// <summary>
        /// PrefixType of the target manages path.
        /// 
        /// BuiltInPrefixTypes class can be used to utilize out of the box SharePoint prefix types.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string PrefixType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Path", Path)
                          .AddRawPropertyValue("PrefixType", PrefixType)

                          .ToString();
        }

        #endregion
    }
}
