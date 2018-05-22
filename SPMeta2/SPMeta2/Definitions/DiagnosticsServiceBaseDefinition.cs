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
    /// Allows to define and deploy custom SharePoint logger inherited from SPDiagnosticsServiceBase. 
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDiagnosticsServiceBase", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class DiagnosticsServiceBaseDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string AssemblyQualifiedName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("AssemblyQualifiedName", AssemblyQualifiedName)

                          .ToString();
        }

        #endregion
    }
}
