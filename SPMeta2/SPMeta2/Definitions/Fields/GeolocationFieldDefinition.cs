using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldGeolocation", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldGeolocation", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [ExpectArrayExtensionMethod]

    public class GeolocationFieldDefinition : FieldDefinition
    {
        #region constructors

        public GeolocationFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Geolocation;
        }

        #endregion
    }
}
