using System;
using System.Collections.Generic;
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
    /// Allows to define and deploy currency field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldCurrency", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldCurrency", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class CurrencyFieldDefinition : FieldDefinition
    {
        #region constructors

        public CurrencyFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Currency;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdateAsLCID]
        [DataMember]
        public int CurrencyLocaleId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<CurrencyFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.CurrencyLocaleId)
                          .ToString();
        }

        #endregion
    }
}
