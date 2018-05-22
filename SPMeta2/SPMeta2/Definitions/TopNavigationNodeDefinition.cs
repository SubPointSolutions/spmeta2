using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint top navigation node.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Navigation.SPNavigationNode", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.NavigationNode", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]

    public class TopNavigationNodeDefinition : NavigationNodeDefinitionBase
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
