using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Standard.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy SharePoint HTML field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Fields.HtmlField", "Microsoft.SharePoint.Publishing")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]


    public class HTMLFieldDefinition : NoteFieldDefinition
    {
        #region constructors

        public HTMLFieldDefinition()
        {
            // Troubleshoot 'HTMLFieldDefinition' - is should render a proper markup for layout pages #739
            // https://github.com/SubPointSolutions/spmeta2/issues/739

            RichText = true;
            RichTextMode = BuiltInRichTextMode.FullHtml;
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
                return BuiltInPublishingFieldTypes.HTML;
            }
            set
            {

            }
        }

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
