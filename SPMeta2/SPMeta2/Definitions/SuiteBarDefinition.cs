using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.Definitions
{

    /// <summary>
    /// Allows to deploy SPOfficialFileHost under target web application
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPWebApplication", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [SingletonIdentity]

    [ParentHostCapability(typeof(WebApplicationDefinition))]
    public class SuiteBarDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        [ExpectValidation]
        public string SuiteBarBrandingElementHtml { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("SuiteBarBrandingElementHtml", SuiteBarBrandingElementHtml)
                          .ToString();
        }

        #endregion
    }
}
