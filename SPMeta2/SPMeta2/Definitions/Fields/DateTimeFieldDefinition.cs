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
    /// Allows to define and deploy datetime field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldDateTime", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldDateTime", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class DateTimeFieldDefinition : FieldDefinition
    {
        #region constructors

        public DateTimeFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.DateTime;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public string CalendarType { get; set; }

        [ExpectValidation]
        public string DisplayFormat { get; set; }

        [ExpectValidation]
        public string FriendlyDisplayFormat { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<DateTimeFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.CalendarType)
                          .AddPropertyValue(p => p.DisplayFormat)
                          .AddPropertyValue(p => p.FriendlyDisplayFormat)
                          .ToString();
        }

        #endregion
    }


}
