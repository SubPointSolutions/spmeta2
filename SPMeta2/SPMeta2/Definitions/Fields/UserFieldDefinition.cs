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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldUser", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldUser", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
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
        public bool AllowMultipleValues { get; set; }

        [ExpectValidation]
        public bool AllowDisplay { get; set; }

        [ExpectValidation]
        public bool Presence { get; set; }

        /// <summary>
        /// ID of the target security group.
        /// Is not used during the provision, must be used maually after the provision to hook up the field with the particular group.
        ///  </summary>
        [ExpectValidation]
        public int? SelectionGroup { get; set; }

        /// <summary>
        /// Refers to SPFieldUserSelectionMode property.
        /// </summary>
        [ExpectValidation]
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
