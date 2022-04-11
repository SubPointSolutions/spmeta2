using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
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
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldLookup", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]
    public class LookupFieldDefinition : FieldDefinition
    {
        #region constructors

        public LookupFieldDefinition()
        {
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

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override string FieldType
        {
            get
            {
                if (AllowMultipleValues)
                    return BuiltInFieldTypes.LookupMulti;

                return BuiltInFieldTypes.Lookup;
            }
            set
            {

            }
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
        /// Url of the target web.
        /// Supports ~sitecollection / ~site tokens.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string LookupWebUrl { get; set; }

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
        [ExpectUpdateAsLookupField]
        public string LookupField { get; set; }

        /// <summary>
        /// References to 'RelationshipDeleteBehavior' property.
        /// None, Cascade, Restrict
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string RelationshipDeleteBehavior { get; set; }

        /// <summary>
        /// Represents'CountRelated' property.
        /// Supported by only SSOM
        /// https://github.com/SubPointSolutions/spmeta2/issues/531
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public bool? CountRelated { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("LookupWebId", LookupWebId)
                          .AddRawPropertyValue("LookupList", LookupList)
                          .AddRawPropertyValue("LookupField", LookupField)
                          .ToString();
        }

        #endregion
    }
}
