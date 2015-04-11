using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy custom SharePoint logger inherited from SPDiagnosticsServiceBase. 
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDiagnosticsServiceBase", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(FarmDefinition))]
    [DefaultParentHostAttribute(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
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
            return new ToStringResult<DiagnosticsServiceBaseDefinition>(this)
                          .AddPropertyValue(p => p.AssemblyQualifiedName)

                          .ToString();
        }

        #endregion
    }
}
