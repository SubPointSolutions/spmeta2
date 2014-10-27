using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;

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
    public class BusinessDataFieldDefinition : FieldDefinition
    {
        #region constructors

        public BusinessDataFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.BusinessData;
        }

        #endregion

        #region properties

        /// <summary>
        /// System instance of the target business data field.
        /// </summary>
        /// 
        [ExpectValidation]

        public string SystemInstanceName { get; set; }

        /// <summary>
        /// Entity namespace of the target business data field
        /// </summary>
        /// 
        [ExpectValidation]

        public string EntityNamespace { get; set; }

        /// <summary>
        /// Entity name of the target business data field
        /// </summary>
        /// 
        [ExpectValidation]

        public string EntityName { get; set; }

        /// <summary>
        /// Name of the the target business data field
        /// </summary>
        /// 
        [ExpectValidation]

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
