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
using SPMeta2.Definitions.Base;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint design package.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUserSolution", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

    public class DesignPackageDefinition : SolutionDefinitionBase
    {
        #region ccontructors

        public DesignPackageDefinition()
        {
            Content = new byte[0];

            Install = true;
            Apply = false;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public int MajorVersion { get; set; }

        [ExpectValidation]
        [DataMember]
        public int MinorVersion { get; set; }

        /// <summary>
        /// Represents DesignPackage.Install() call
        /// True by default.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool Install { get; set; }

        /// <summary>
        /// Represents DesignPackage.Apply() call
        /// False by default.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool Apply { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FileName", FileName)
                          .AddRawPropertyValue("SolutionId", SolutionId)
                          .AddRawPropertyValue("Install", Install)
                          .AddRawPropertyValue("Apply", Apply)
                          .ToString();
        }

        #endregion
    }
}
