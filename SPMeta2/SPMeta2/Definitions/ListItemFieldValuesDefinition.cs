using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy field value on the target list item.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListItemDefinition))]

    [Serializable] 
    [DataContract]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListItemDefinition))]
    public class ListItemFieldValuesDefinition : DefinitionBase
    {
        #region constructors

        public ListItemFieldValuesDefinition()
        {
            Values = new List<FieldValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [IdentityKey]
        public List<FieldValue> Values { set; get; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .ToString();
        }

        #endregion
    }
}
