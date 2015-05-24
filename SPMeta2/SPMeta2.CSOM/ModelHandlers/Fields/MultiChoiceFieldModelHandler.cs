using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldMultiChoice);
        }


        #endregion

        #region methods


        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<MultiChoiceFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field.Context.CastTo<FieldMultiChoice>(field);

            typedField.FillInChoice = typedFieldModel.FillInChoice;

            if (typedFieldModel.Choices.Count > 0)
            {
                typedField.Choices = typedFieldModel.Choices.ToArray();
            }
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

            if (typedFieldModel.Mappings.Count > 0)
            {
                var mappingsNode = new XElement("MAPPINGS");
                var currentValueIndex = 1;

                foreach (var mappingValue in typedFieldModel.Mappings)
                {
                    var mappingValueNode = new XElement("MAPPING", mappingValue);
                    mappingValueNode.SetAttributeValue("Value", currentValueIndex);

                    mappingsNode.Add(mappingValueNode);

                    currentValueIndex++;
                }

                fieldTemplate.Add(mappingsNode);
            }
        }

        #endregion
    }
}
