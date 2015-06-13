using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class DependentLookupFieldDefinitionValidator : DependentLookupFieldModelHandler
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.InternalName, o => o.InternalName);

            // skip base lookup field props
            assert.SkipProperty(m => m.AddFieldOptions, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.AdditionalAttributes, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.AddToDefaultView, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.AllowDeletion, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.AllowMultipleValues, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.DefaultValue, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.EnforceUniqueValues, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.FieldType, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.Hidden, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.Id, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.Indexed, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.JSLink, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.LookupList, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.LookupListTitle, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.LookupListUrl, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.LookupWebId, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.RawXml, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.Required, "DependentLookupFieldDefinition");

            assert.SkipProperty(m => m.ShowInDisplayForm, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ShowInEditForm, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ShowInListSettings, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ShowInNewForm, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ShowInVersionHistory, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ShowInViewForms, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.StaticName, "DependentLookupFieldDefinition");

            assert.SkipProperty(m => m.ValidationFormula, "DependentLookupFieldDefinition");
            assert.SkipProperty(m => m.ValidationMessage, "DependentLookupFieldDefinition");

            if (!string.IsNullOrEmpty(definition.Group))
                assert.ShouldBeEqual(m => m.Group, o => o.Group);
            else
                assert.SkipProperty(m => m.Group);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description);


            if (!string.IsNullOrEmpty(definition.LookupField))
                assert.ShouldBeEqual(m => m.LookupField, o => o.LookupField);
            else
                assert.SkipProperty(m => m.LookupField);

            // binding

            if (definition.PrimaryLookupFieldId.HasGuidValue())
            {
                var primaryLookupField = GetPrimaryField(modelHost, definition);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldId);
                    var isValid = primaryLookupField.Id == definition.PrimaryLookupFieldId.Value;

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
                assert.SkipProperty(m => m.PrimaryLookupFieldId, "Value is null or empty.");

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldInternalName))
            {
                var primaryLookupField = GetPrimaryField(modelHost, definition);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldInternalName);
                    var isValid = primaryLookupField.InternalName == definition.PrimaryLookupFieldInternalName;

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
                assert.SkipProperty(m => m.PrimaryLookupFieldInternalName);

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldTitle))
            {
                var primaryLookupField = GetPrimaryField(modelHost, definition);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldTitle);
                    var isValid = primaryLookupField.Title == definition.PrimaryLookupFieldTitle;

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
                assert.SkipProperty(m => m.PrimaryLookupFieldTitle);

        }
    }
}
