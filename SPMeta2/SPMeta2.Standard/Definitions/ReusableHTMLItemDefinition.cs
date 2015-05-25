using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class ReusableHTMLItemDefinition : ReusableItemDefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public string ReusableHTML { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ReusableHTMLItemDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.ReusableHTML)
                          .ToString();
        }

        #endregion
    }
}
