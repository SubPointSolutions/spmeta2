using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
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

            var context = spObject.Context;

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

            assert.SkipProperty(m => m.RelationshipDeleteBehavior, "RelationshipDeleteBehavior");

            // web url
            if (!string.IsNullOrEmpty(definition.LookupWebUrl))
            {
                // TODO
                throw new SPMeta2NotImplementedException("definition.LookupWebUrl");
            }
            else
            {
                assert.SkipProperty(m => m.LookupWebUrl, "LookupWebUrl");
            }

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

            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource", "DescriptionResource"
            });

            if (supportsLocalization)
            {
                if (definition.TitleResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.TitleResource);
                        var isValid = true;

                        foreach (var userResource in s.TitleResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "TitleResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                    .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
                        }

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
                    assert.SkipProperty(m => m.TitleResource, "TitleResource is NULL or empty. Skipping.");
                }

                if (definition.DescriptionResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.DescriptionResource);
                        var isValid = true;

                        foreach (var userResource in s.DescriptionResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "DescriptionResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                       .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
                        }

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
                    assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is NULL or empty. Skipping.");
                }

            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
                assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is null or empty. Skipping.");
            }
        }
    }
}
