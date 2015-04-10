using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Identity;

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
    [DataContract]
    [ExpectWithExtensionMethod]
    public class WebConfigModificationDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public string Path { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public uint Sequence { get; set; }

        [ExpectValidation]
        [DataMember]
        public string Owner { get; set; }

        [ExpectValidation]
        [DataMember]
        public string Type { get; set; }

        [ExpectValidation]
        [DataMember]
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
