using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy currency field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldCurrency", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldCurrency", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

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
        [ExpectRequired]
        [DataMember]
        public override sealed string FieldType { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsLCID]
        [DataMember]
        public int CurrencyLocaleId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("CurrencyLocaleId", CurrencyLocaleId)
                          .ToString();
        }

        #endregion
    }
}
