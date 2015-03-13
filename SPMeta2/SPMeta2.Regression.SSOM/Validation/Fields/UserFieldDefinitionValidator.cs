using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class UserFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(UserFieldDefinition);
            }
        }

        protected override void CustomFieldTypeValidation(AssertPair<FieldDefinition, SPField> assert, SPField spObject, FieldDefinition definition)
        {
            var typedObject = spObject as SPFieldUser;
            var typedDefinition = definition.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.FieldType);
                var dstProp = d.GetExpressionValue(m => d.TypeAsString);

                var isValid = typedDefinition.AllowMultipleValues
                    ? typedObject.TypeAsString == "UserMulti"
                    : typedObject.TypeAsString == "User";

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var typedField = spObject as SPFieldUser;
            var typedDefinition = model.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.ShouldBeEqual(m => m.AllowMultipleValues, o => o.AllowMultipleValues);
            typedFieldAssert.ShouldBeEqual(m => m.AllowDisplay, o => o.AllowDisplay);
            typedFieldAssert.ShouldBeEqual(m => m.Presence, o => o.Presence);
            typedFieldAssert.ShouldBeEqual(m => m.SelectionMode, o => o.GetSelectionMode());

            if (typedDefinition.SelectionGroup.HasValue)
            {
                typedFieldAssert.ShouldBeEqual(m => m.SelectionGroup, o => o.SelectionGroup);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.SelectionGroup, "SelectionGroup is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.SelectionGroupName))
            {
                var web = ExtractWebFromHost(modelHost);
                var group = web.SiteGroups.GetByName(typedDefinition.SelectionGroupName);

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.SelectionGroupName);
                    var isValid = typedField.SelectionGroup == group.ID;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.SelectionGroupName, "SelectionGroupName is NULL. Skipping.");
            }
        }
    }

    internal static class SPFieldUserUtils
    {
        public static string GetSelectionMode(this SPFieldUser field)
        {
            return field.SelectionMode.ToString();
        }
    }
}
