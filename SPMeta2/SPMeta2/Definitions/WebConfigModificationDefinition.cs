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
    /// Allows to define and deploy SharePoint web configuration modifications.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPWebConfigModification", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(WebApplicationDefinition))]
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
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Owner", Owner)
                          .AddRawPropertyValue("Path", Path)
                          .AddRawPropertyValue("Sequence", Sequence)
                          .AddRawPropertyValue("Type", Type)
                          .AddRawPropertyValue("Value", Value)
                          .ToString();
        }

        #endregion
    }
}
