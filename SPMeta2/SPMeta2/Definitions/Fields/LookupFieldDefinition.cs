using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy boolean field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldLookup", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldLookup", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class LookupFieldDefinition : FieldDefinition
    {
        #region constructors

        public LookupFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Lookup;
            LookupField = BuiltInInternalFieldNames.Title;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public bool AllowMultipleValues { get; set; }

        /// <summary>
        /// ID of the target web.
        /// </summary>
        [ExpectValidation]
        public Guid? LookupWebId { get; set; }

        /// <summary>
        /// Name or GUID of the target list.
        /// Could be "Self", "UserInfo" or ID of the target list.
        /// </summary>
        [ExpectValidation]
        public string LookupList { get; set; }

        /// <summary>
        /// References to 'ShowField' property.
        /// Should be an internal name of the target field.
        /// </summary>
        [ExpectValidation]
        public string LookupField { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<LookupFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.LookupWebId)
                          .AddPropertyValue(p => p.LookupList)
                          .AddPropertyValue(p => p.LookupField)
                          .ToString();
        }

        #endregion
    }
}
