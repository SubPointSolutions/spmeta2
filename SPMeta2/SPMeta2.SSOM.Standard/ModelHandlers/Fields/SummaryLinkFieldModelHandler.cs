using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Fields
{
    public class SummaryLinkFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SummaryLinkFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SummaryLinkField);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<SummaryLinkFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SummaryLinkField;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<SummaryLinkFieldDefinition>("model", value => value.RequireNotNull());

            // TODO
            //fieldTemplate.SetAttribute(BuiltInFieldAttributes.RichText, typedFieldModel.RichText.ToString().ToUpper());
        }

        #endregion
    }
}
