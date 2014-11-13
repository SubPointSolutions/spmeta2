using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class MultiChoiceFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MultiChoiceFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldMultiChoice);
        }


        #endregion

        #region methods


        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<MultiChoiceFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.FillInChoice, typedFieldModel.FillInChoice);

            if (typedFieldModel.Choices.Count > 0)
            {
                var choicesNode = new XElement("CHOICES");

                foreach (var choice in typedFieldModel.Choices)
                {
                    choicesNode.Add(new XElement("CHOICE", choice));
                }

                fieldTemplate.Add(choicesNode);
            }
        }

        #endregion
    }
}
