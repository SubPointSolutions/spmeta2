using System;
using System.Runtime.Serialization;
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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldUser", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldUser", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]

    public class UserFieldDefinition : FieldDefinition
    {
        #region constructors

        public UserFieldDefinition()
        {
            AllowDisplay = true;
            SelectionMode = BuiltInFieldUserSelectionMode.PeopleAndGroups;
        }

        #endregion

        #region properties

        /// <summary>
        /// References to 'ShowField' property.
        /// Should be an internal name of the target field.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsLookupField]
        public string LookupField { get; set; }

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override string FieldType
        {
            get
            {
                if (AllowMultipleValues)
                    return BuiltInFieldTypes.UserMulti;

                return BuiltInFieldTypes.User;
            }
            set
            {

            }
        }

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
        [ExpectUpdate]
        [DataMember]
        public bool AllowMultipleValues { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool AllowDisplay { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool Presence { get; set; }

        /// <summary>
        /// ID of the target security group.
        ///  </summary>
        [ExpectValidation]
        [DataMember]
        public int? SelectionGroup { get; set; }

        /// <summary>
        /// Name of the target security group.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string SelectionGroupName { get; set; }

        /// <summary>
        /// Refers to SPFieldUserSelectionMode property.
        /// </summary>
        [ExpectValidation]
        [ExpectUpdateAsFieldUserSelectionMode]
        [DataMember]
        public string SelectionMode { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("AllowDisplay", AllowDisplay)
                          .AddRawPropertyValue("Presence", Presence)
                          .AddRawPropertyValue("SelectionGroup", SelectionGroup)
                          .AddRawPropertyValue("SelectionMode", SelectionMode)
                          .ToString();
        }

        #endregion
    }
}
