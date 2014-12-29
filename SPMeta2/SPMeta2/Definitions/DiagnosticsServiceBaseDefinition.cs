using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy custom SharePoint logger inherited from SPDiagnosticsServiceBase. 
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDiagnosticsServiceBase", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(FarmDefinition))]
    [DefaultParentHostAttribute(typeof(FarmDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    public class DiagnosticsServiceBaseDefinition : DefinitionBase
    {
        #region properties

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
