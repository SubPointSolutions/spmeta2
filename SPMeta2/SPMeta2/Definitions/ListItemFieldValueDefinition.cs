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
    /// Allows to define and deploy field value on the target list item.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListItemDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListItemDefinition))]
    public class ListItemFieldValueDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Field name of the target field value.
        /// FieldId property can be used to set field by ID.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Field")]
        [DataMember]
        [IdentityKey]
        public string FieldName { get; set; }

        /// <summary>
        /// Field id.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired(GroupName = "Field")]
        [DataMember]
        [IdentityKey]
        public Guid? FieldId { get; set; }

        /// <summary>
        /// Target field value.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public object Value { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FieldName", FieldName)
                          .AddRawPropertyValue("FieldId", FieldId)
                          .ToString();
        }

        #endregion
    }
}
