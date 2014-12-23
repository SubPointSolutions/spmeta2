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
    /// Allows to define and deploy SharePoint web configuration modifications.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPWebConfigModification", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]
    [Serializable]
    [ExpectWithExtensionMethod]
    public class WebConfigModificationDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        public string Path { get; set; }

        [ExpectValidation]
        public string Name { get; set; }

        [ExpectValidation]
        public uint Sequence { get; set; }

        [ExpectValidation]
        public string Owner { get; set; }

        [ExpectValidation]
        public string Type { get; set; }

        [ExpectValidation]
        public string Value { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebConfigModificationDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Owner)
                          .AddPropertyValue(p => p.Path)
                          .AddPropertyValue(p => p.Sequence)
                          .AddPropertyValue(p => p.Type)
                          .AddPropertyValue(p => p.Value)
                          .ToString();
        }

        #endregion
    }
}
