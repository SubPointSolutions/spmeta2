using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    ///  Allows to attach field to the target list.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPField", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class ListFieldLinkDefinition : DefinitionBase
    {
        #region properties

        #region properties

        /// <summary>
        /// ID of the target field to be attached to content type.
        /// BuiltInFieldId class can be used to utilize out of the box SharePoint fields. 
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired(GroupName = "FieldIdOrName")]
        [DataMember]
        [IdentityKey]
        public Guid? FieldId { get; set; }

        [ExpectValidation]
        [ExpectRequired(GroupName = "FieldIdOrName")]
        [DataMember]
        [IdentityKey]
        public string FieldInternalName { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? Required { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Is not supported in CSOM yet!
        /// https://officespdev.uservoice.com/forums/224641-general/suggestions/7024931-enhance-fieldlink-class-with-additional-properties
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public string DisplayName { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool AddToDefaultView { get; set; }

        [ExpectValidation]
        [DataMember]
        public BuiltInAddFieldOptions AddFieldOptions { get; set; }

        #endregion

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FieldId", FieldId)
                          .ToString();
        }

        #endregion
    }
}
