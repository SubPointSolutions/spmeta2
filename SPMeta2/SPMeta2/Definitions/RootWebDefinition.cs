using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define SharePoint root web. Used to built up side model with root web provision included.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    public class RootWebDefinition : DefinitionBase
    {
        #region methods

        public override string ToString()
        {
            return string.Format("RootWeb Definition has not properties.");
        }

        #endregion
    }
}
