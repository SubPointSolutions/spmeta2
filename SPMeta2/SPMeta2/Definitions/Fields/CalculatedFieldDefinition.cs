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
    /// Allows to define and deploy calculated field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldCalculated", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldCalculated", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class CalculatedFieldDefinition : FieldDefinition
    {
        #region constructors

        public CalculatedFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Calculated;
        }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<CalculatedFieldDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
