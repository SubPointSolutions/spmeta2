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
        }

        #endregion

        #region properties

        [ExpectValidation]
        public Guid? LookupWebId { get; set; }

        [ExpectValidation]
        public string LookupList { get; set; }

        [ExpectValidation]
        public string LookupField { get; set; }

        [ExpectValidation]
        public string SelectionMode { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<LookupFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.LookupWebId)
                          .AddPropertyValue(p => p.LookupList)
                          .AddPropertyValue(p => p.LookupField)
                          .AddPropertyValue(p => p.SelectionMode)
                          .ToString();
        }

        #endregion
    }
}
