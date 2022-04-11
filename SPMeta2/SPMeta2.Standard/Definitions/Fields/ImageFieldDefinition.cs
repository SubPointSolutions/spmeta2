using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy SharePoint image field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Fields.ImageField", "Microsoft.SharePoint.Publishing")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]


    public class ImageFieldDefinition : FieldDefinition
    {
        #region constructors

        public ImageFieldDefinition()
        {
            FieldType = BuiltInPublishingFieldTypes.Image;

            // RichTextMode/RichText should be set as follow to make sure field can be edited and displayed correctly
            // Skipping these atts would resul a pure HTML string on the publishing page layout

            // Enhance 'ImageFieldDefinition' - add default AdditionalAttributes #552
            // https://github.com/SubPointSolutions/spmeta2/issues/552
            AdditionalAttributes.Add(new FieldAttributeValue { Name = "RichTextMode", Value = "FullHtml" });
            AdditionalAttributes.Add(new FieldAttributeValue { Name = "RichText", Value = "TRUE" });
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
