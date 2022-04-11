using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;
using System;

namespace SPMeta2.Regression.CSOM.Validation.Fields
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
            // CSOM provision for DependentLookupFieldDefinition does not update these props
            // seems to be a by design SharePoin issue
            // https://github.com/SubPointSolutions/spmeta2/issues/753

            this.SkipAllowMultipleValuesValidation = true;
            this.SkipFieldTypeValidation = true;
            this.SkipLookupFieldValidation = true;

            this.ModelHost = modelHost;

            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);
            var typedField = spObject.Context.CastTo<FieldLookup>(spObject);
            var typedDefinition = model.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());
            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            var context = spObject.Context;

            typedFieldAssert.SkipProperty(m => m.RelationshipDeleteBehavior, "DependentLookupFieldDefinition");

            // binding
            var fields = FieldLookupService.GetFieldCollection(this.ModelHost);

            FieldLookup primaryLookupField = null;

            if (typedDefinition.PrimaryLookupFieldId.HasGuidValue())
            {
                primaryLookupField = FieldLookupService.GetFieldAs<FieldLookup>(fields, typedDefinition.PrimaryLookupFieldId.Value, null, null);
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
                primaryLookupField = FieldLookupService.GetFieldAs<FieldLookup>(fields, null, typedDefinition.PrimaryLookupFieldInternalName, null);
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
                primaryLookupField = FieldLookupService.GetFieldAs<FieldLookup>(fields, null, null, typedDefinition.PrimaryLookupFieldTitle);
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
    }
}
