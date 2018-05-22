using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to delete quick navigation nodes.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

    //[ExpectManyInstances]
    [SingletonIdentity]
    public class DeleteQuickLaunchNavigationNodesDefinition : DeleteNavigationNodesDefinitionBase
    {
        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .ToString();
        }

        #endregion
    }
}
