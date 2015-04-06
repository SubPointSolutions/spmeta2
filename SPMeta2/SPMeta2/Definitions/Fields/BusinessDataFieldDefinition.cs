using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy business data field.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPBusinessDataField", "Microsoft.SharePoint")]
    //[SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

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
            return new ToStringResult<BusinessDataFieldDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.InternalName)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.Group)

                          .AddPropertyValue(p => p.SystemInstanceName)
                          .AddPropertyValue(p => p.EntityNamespace)
                          .AddPropertyValue(p => p.EntityName)
                          .AddPropertyValue(p => p.BdcFieldName)
                          .ToString();
        }

        #endregion
    }
}
