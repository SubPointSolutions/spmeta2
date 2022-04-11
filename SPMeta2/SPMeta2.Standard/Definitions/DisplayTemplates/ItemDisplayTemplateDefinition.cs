using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions.DisplayTemplates
{
    /// <summary>
    /// Allows to define and deploy item display template.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class ItemDisplayTemplateDefinition : ItemControlTemplateDefinitionBase
    {
        #region constructors

        public ItemDisplayTemplateDefinition()
        {
        }

        #endregion

        #region properties

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ManagedPropertyMappings { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())

                          .ToString();
        }

        #endregion
    }
}
