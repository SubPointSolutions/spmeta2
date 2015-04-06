using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint top navigation node.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Navigation.SPNavigationNode", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.NavigationNode", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    public class TopNavigationNodeDefinition : NavigationNodeDefinitionBase
    {
        #region methods

        public override string ToString()
        {
            return new ToStringResult<TopNavigationNodeDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
