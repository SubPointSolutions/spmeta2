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
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldGeolocation", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldGeolocation", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    public class GeolocationFieldDefinition : FieldDefinition
    {
        #region constructors

        public GeolocationFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Geolocation;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override sealed string FieldType { get; set; }

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

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())

                          .ToString();
        }

        #endregion
    }
}
