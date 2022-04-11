using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy text field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldText", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldText", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]

    public class TextFieldDefinition : FieldDefinition
    {
        #region constructors

        public TextFieldDefinition()
        {
        }

        #endregion

        #region properties


        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override string FieldType
        {
            get
            {
                return BuiltInFieldTypes.Text;
            }
            set
            {

            }
        }

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 100)]
        [DataMember]
        public int? MaxLength { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("MaxLength", MaxLength)
                          .ToString();
        }

        #endregion
    }
}
