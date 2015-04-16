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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldUser", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldUser", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class UserFieldDefinition : FieldDefinition
    {
        #region constructors

        public UserFieldDefinition()
        {
            AllowDisplay = true;
            FieldType = BuiltInFieldTypes.User;
            SelectionMode = BuiltInFieldUserSelectionMode.PeopleAndGroups;
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
            return new ToStringResult<UserFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.AllowDisplay)
                          .AddPropertyValue(p => p.Presence)
                          .AddPropertyValue(p => p.SelectionGroup)
                          .AddPropertyValue(p => p.SelectionMode)
                          .ToString();
        }

        #endregion
    }
}
