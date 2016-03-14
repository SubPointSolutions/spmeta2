using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class DependentLookupFieldDefinitionValidator : LookupFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(DependentLookupFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var typedField = spObject as SPFieldLookup;
            var typedDefinition = model.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.SkipProperty(m => m.RelationshipDeleteBehavior, "DependentLookupFieldDefinition");

            if (!string.IsNullOrEmpty(typedDefinition.LookupField))
                typedFieldAssert.ShouldBeEqual(m => m.LookupField, o => o.LookupField);
            else
                typedFieldAssert.SkipProperty(m => m.LookupField);

            // binding
            var fields = FieldLookupService.GetFieldCollection(this.ModelHost);

            SPFieldLookup primaryLookupField = null;

            if (typedDefinition.PrimaryLookupFieldId.HasGuidValue())
            {
                primaryLookupField = FieldLookupService.GetFieldAs<SPFieldLookup>(fields, typedDefinition.PrimaryLookupFieldId.Value, null, null);
                typedFieldAssert.ShouldNotBeNull(primaryLookupField);

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldId);
                    var isValid = primaryLookupField.Id == new Guid(typedField.PrimaryFieldId);

                    return new PropertyValidationResult { Tag = p.Tag, Src = srcProp, Dst = null, IsValid = isValid };
                });
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.PrimaryLookupFieldId, "PrimaryLookupFieldId is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.PrimaryLookupFieldInternalName))
            {
                primaryLookupField = FieldLookupService.GetFieldAs<SPFieldLookup>(fields, null, typedDefinition.PrimaryLookupFieldInternalName, null);
                typedFieldAssert.ShouldNotBeNull(primaryLookupField);

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldInternalName);
                    var isValid = primaryLookupField.Id == new Guid(typedField.PrimaryFieldId);

                    return new PropertyValidationResult { Tag = p.Tag, Src = srcProp, Dst = null, IsValid = isValid };
                });
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.PrimaryLookupFieldInternalName, "PrimaryLookupFieldInternalName is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.PrimaryLookupFieldTitle))
            {
                primaryLookupField = FieldLookupService.GetFieldAs<SPFieldLookup>(fields, null, null, typedDefinition.PrimaryLookupFieldTitle);
                typedFieldAssert.ShouldNotBeNull(primaryLookupField);

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PrimaryLookupFieldTitle);
                    var isValid = primaryLookupField.Id == new Guid(typedField.PrimaryFieldId);

                    return new PropertyValidationResult { Tag = p.Tag, Src = srcProp, Dst = null, IsValid = isValid };
                });
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.PrimaryLookupFieldTitle, "PrimaryLookupFieldInternalName is NULL. Skipping.");
            }
        }

        protected virtual SPFieldLookup GetPrimaryField(DependentLookupFieldDefinition definition)
        {
            var fields = FieldLookupService.GetFieldCollection(this.ModelHost);

            return FieldLookupService.GetFieldAs<SPFieldLookup>(
                fields, definition.PrimaryLookupFieldId, definition.PrimaryLookupFieldInternalName, definition.PrimaryLookupFieldTitle);
        }
    }
}
