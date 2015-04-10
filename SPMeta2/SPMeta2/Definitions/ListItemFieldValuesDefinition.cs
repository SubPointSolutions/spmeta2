using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy field value on the target list item.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListItemDefinition))]

    [Serializable] 
    [DataContract]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

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
            return new ToStringResult<ListItemFieldValuesDefinition>(this)
                          .ToString();
        }

        #endregion
    }
}
