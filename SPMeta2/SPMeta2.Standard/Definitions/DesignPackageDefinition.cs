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
    /// Allows to define and deploy SharePoint design package.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.DesignPackageInfo", "Microsoft.SharePoint.Publishing")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Publishing.DesignPackageInfo", "Microsoft.SharePoint.Client.Publishing")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

    public class DesignPackageDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string PackageName { get; set; }

        [ExpectRequired]
        [DataMember]
        [ExpectValidation]
        public Guid PackageGuid { get; set; }

        [ExpectValidation]
        [DataMember]
        public int? MajorVersion { get; set; }

        [ExpectValidation]
        [DataMember]
        public int? MinorVersion { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<DesignPackageDefinition>(this)
                          .AddPropertyValue(p => p.PackageName)
                          .ToString();
        }

        #endregion
    }
}
