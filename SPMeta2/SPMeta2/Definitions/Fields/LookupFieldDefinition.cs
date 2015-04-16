using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class LookupFieldDefinition : FieldDefinition
    {
        #region constructors

        public LookupFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Lookup;
            LookupField = BuiltInInternalFieldNames.Title;
        }

        #endregion

        #region overrides

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
        /// Returns false if AllowMultipleValues = true.
        /// Multi lookup field does not support Indexed = trur flag and would give an exception.
        /// </summary>
        /// 
        [DataMember]

        public override bool Indexed
        {
            get
            {
                if (AllowMultipleValues)
                    return false;

                return base.Indexed;
            }
            set
            {
                if (!AllowMultipleValues)
                    base.Indexed = value;
            }
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool AllowMultipleValues { get; set; }

        /// <summary>
        /// ID of the target web.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public Guid? LookupWebId { get; set; }

        /// <summary>
        /// Name or GUID of the target list.
        /// Could be "Self", "UserInfo" or ID of the target list.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string LookupList { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string LookupListTitle { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string LookupListUrl { get; set; }

        /// <summary>
        /// References to 'ShowField' property.
        /// Should be an internal name of the target field.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
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
