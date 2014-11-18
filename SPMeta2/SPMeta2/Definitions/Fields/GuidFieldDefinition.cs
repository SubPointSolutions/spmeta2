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
    /// Allows to define and deploy guid field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldGuid", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldGuid", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class GuidFieldDefinition : FieldDefinition
    {
        #region constructors

        public GuidFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Guid;
        }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<GuidFieldDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
