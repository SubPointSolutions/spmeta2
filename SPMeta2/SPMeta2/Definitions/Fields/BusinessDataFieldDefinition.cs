using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy business data field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPBusinessDataField", "Microsoft.SharePoint")]
    //[SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    public class BusinessDataFieldDefinition : FieldDefinition
    {
        #region constructors

        public BusinessDataFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.BusinessData;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override sealed string FieldType { get; set; }

        [ExpectValidation]
        [DataMember]
        public override string ValidationMessage
        {
            get { return string.Empty; }
            set { }
        }

        [ExpectValidation]
        [DataMember]
        public override string ValidationFormula
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// System instance of the target business data field.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string SystemInstanceName { get; set; }

        /// <summary>
        /// Entity namespace of the target business data field
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string EntityNamespace { get; set; }

        /// <summary>
        /// Entity name of the target business data field
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string EntityName { get; set; }

        /// <summary>
        /// Name of the the target business data field
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string BdcFieldName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("Description", Description)
                          .AddRawPropertyValue("InternalName", InternalName)
                          .AddRawPropertyValue("Id", Id)
                          .AddRawPropertyValue("Group", Group)

                          .AddRawPropertyValue("SystemInstanceName", SystemInstanceName)
                          .AddRawPropertyValue("EntityNamespace", EntityNamespace)
                          .AddRawPropertyValue("EntityName", EntityName)
                          .AddRawPropertyValue("BdcFieldName", BdcFieldName)
                          .ToString();
        }

        #endregion
    }
}
