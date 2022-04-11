﻿using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Xml.Linq;
using System.Linq;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            HostList = ExtractListFromHost(modelHost);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);
        }

        protected bool IsListScopedField
        {
            get { return HostList != null; }
        }
        protected SPList HostList { get; set; }

        protected virtual void CustomFieldTypeValidation(AssertPair<FieldDefinition, SPField> assert, SPField spObject,
            FieldDefinition definition)
        {
            assert.ShouldBeEqual(m => m.FieldType, o => o.TypeAsString);
        }

        protected void ValidateField(AssertPair<FieldDefinition, SPField> assert, SPField spObject, FieldDefinition definition)
        {
            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                //.ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required);
            //.ShouldBeEqual(m => m.Description, o => o.Description)
            //.ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
            //.ShouldBeEqual(m => m.Group, o => o.Group);

            if (!string.IsNullOrEmpty(definition.Group))
                assert.ShouldBeEqual(m => m.Group, o => o.Group);
            else
                assert.SkipProperty(m => m.Group);

            if (!string.IsNullOrEmpty(definition.StaticName))
                assert.ShouldBeEqual(m => m.StaticName, o => o.StaticName);
            else
                assert.SkipProperty(m => m.StaticName);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description);

            if (definition is LookupFieldDefinition)
            {
                var depLookupDefinition = definition as LookupFieldDefinition;

                // cjeck against CountRelated for lookups
                if (depLookupDefinition.CountRelated.HasValue
                    && depLookupDefinition.ReadOnlyField.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ReadOnlyField);

                        var isValid = (bool)srcProp.Value == depLookupDefinition.CountRelated.Value;

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
                    assert.SkipProperty(m => m.ReadOnlyField, "CountRelated / ReadOnlyField is null or empty");

                    //if (definition.ReadOnlyField.HasValue)
                    //    assert.ShouldBeEqual(m => m.ReadOnlyField, o => o.ReadOnlyField);
                    //else
                    //    assert.SkipProperty(m => m.ReadOnlyField, "ReadOnlyField is null or empty");
                }
            }
            else
            {
                if (definition.ReadOnlyField.HasValue)
                    assert.ShouldBeEqual(m => m.ReadOnlyField, o => o.ReadOnlyField);
                else
                    assert.SkipProperty(m => m.ReadOnlyField, "ReadOnlyField is null or empty");
            }

            CustomFieldTypeValidation(assert, spObject, definition);

            if (definition.AddFieldOptions.HasFlag(BuiltInAddFieldOptions.DefaultValue))
            {
                assert.SkipProperty(m => m.AddFieldOptions, "BuiltInAddFieldOptions.DefaultValue. Skipping.");
            }
            else
            {
                // TODO
            }

            if (definition.AddToDefaultView)
            {
                if (IsListScopedField)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.AddToDefaultView);
                        var field = HostList.Fields[definition.Id];

                        var isValid = HostList.DefaultView
                            .ViewFields
                            .ToStringCollection()
                            .Contains(field.InternalName);

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
                    assert.SkipProperty(m => m.AddToDefaultView, "IsListScopedField = true. AddToDefaultView is ignored. Skipping.");
                }
            }
            else
            {
                assert.SkipProperty(m => m.AddToDefaultView, "AddToDefaultView is false. Skipping.");
            }

            if (definition.AdditionalAttributes.Count == 0)
            {
                assert.SkipProperty(m => m.AdditionalAttributes, "AdditionalAttributes count is 0. Skipping.");
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.AdditionalAttributes);
                    var isValid = true;

                    var dstXmlNode = XDocument.Parse(d.SchemaXml).Root;

                    foreach (var attr in s.AdditionalAttributes)
                    {
                        var sourceAttrName = attr.Name;
                        var sourceAttrValue = attr.Value;

                        var destAttrValue = dstXmlNode.GetAttributeValue(sourceAttrName);

                        isValid = sourceAttrValue == destAttrValue;

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

            if (string.IsNullOrEmpty(definition.RawXml))
            {
                assert.SkipProperty(m => m.RawXml, "RawXml is NULL or empty. Skipping.");
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.RawXml);
                    var isValid = true;

                    var srcXmlNode = XDocument.Parse(s.RawXml).Root;
                    var dstXmlNode = XDocument.Parse(d.SchemaXml).Root;

                    foreach (var attr in srcXmlNode.Attributes())
                    {
                        var sourceAttrName = attr.Name.LocalName;
                        var sourceAttrValue = attr.Value;

                        var destAttrValue = dstXmlNode.GetAttributeValue(sourceAttrName);

                        isValid = sourceAttrValue == destAttrValue;

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

            // TODO, R&D to check InternalName changes in list-scoped fields
            if (spObject.InternalName == definition.InternalName)
            {
                assert.ShouldBeEqual(m => m.InternalName, o => o.InternalName);
            }
            else
            {
                assert.SkipProperty(m => m.InternalName,
                    "Target InternalName is different to source InternalName. Could be an error if this is not a list scoped field");
            }

            assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);

            if (definition.EnforceUniqueValues.HasValue)
                assert.ShouldBeEqual(m => m.EnforceUniqueValues, o => o.EnforceUniqueValues);
            else
                assert.SkipProperty(m => m.EnforceUniqueValues, "EnforceUniqueValues is NULL");


            if (!string.IsNullOrEmpty(definition.ValidationFormula))
                assert.ShouldBeEqual(m => m.ValidationFormula, o => o.ValidationFormula);
            else
                assert.SkipProperty(m => m.ValidationFormula, string.Format("ValidationFormula value is not set. Skippping."));

            if (!string.IsNullOrEmpty(definition.ValidationMessage))
                assert.ShouldBeEqual(m => m.ValidationMessage, o => o.ValidationMessage);
            else
                assert.SkipProperty(m => m.ValidationMessage, string.Format("ValidationFormula value is not set. Skippping."));

            if (!string.IsNullOrEmpty(definition.DefaultValue))
                assert.ShouldBePartOf(m => m.DefaultValue, o => o.DefaultValue);
            else
                assert.SkipProperty(m => m.DefaultValue, string.Format("Default value is not set. Skippping."));

            if (!string.IsNullOrEmpty(definition.DefaultFormula))
                assert.ShouldBePartOf(m => m.DefaultFormula, o => o.DefaultFormula);
            else
                assert.SkipProperty(m => m.DefaultFormula, string.Format("Default formula is not set. Skippping."));

            if (!string.IsNullOrEmpty(spObject.JSLink) &&
                (spObject.JSLink == "SP.UI.Taxonomy.js|SP.UI.Rte.js(d)|SP.Taxonomy.js(d)|ScriptForWebTaggingUI.js(d)" ||
                spObject.JSLink == "choicebuttonfieldtemplate.js" ||
                spObject.JSLink == "clienttemplates.js"))
            {
                assert.SkipProperty(m => m.JSLink, string.Format("OOTB read-ony JSLink value:[{0}]", spObject.JSLink));
            }
            else
            {
                assert.ShouldBePartOf(m => m.JSLink, o => o.JSLink);
            }

            if (definition.ShowInDisplayForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInDisplayForm, o => o.ShowInDisplayForm);
            else
                assert.SkipProperty(m => m.ShowInDisplayForm, "ShowInDisplayForm is NULL");

            if (definition.ShowInEditForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInEditForm, o => o.ShowInEditForm);
            else
                assert.SkipProperty(m => m.ShowInEditForm, "ShowInEditForm is NULL");

            if (definition.ShowInListSettings.HasValue)
                assert.ShouldBeEqual(m => m.ShowInListSettings, o => o.ShowInListSettings);
            else
                assert.SkipProperty(m => m.ShowInListSettings, "ShowInListSettings is NULL");

            if (definition.ShowInNewForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInNewForm, o => o.ShowInNewForm);
            else
                assert.SkipProperty(m => m.ShowInNewForm, "ShowInNewForm is NULL");

            if (definition.ShowInVersionHistory.HasValue)
                assert.ShouldBeEqual(m => m.ShowInVersionHistory, o => o.ShowInVersionHistory);
            else
                assert.SkipProperty(m => m.ShowInVersionHistory, "ShowInVersionHistory is NULL");

            if (definition.ShowInViewForms.HasValue)
                assert.ShouldBeEqual(m => m.ShowInViewForms, o => o.ShowInViewForms);
            else
                assert.SkipProperty(m => m.ShowInViewForms, "ShowInViewForms is NULL");

            assert
                .ShouldBeEqual(m => m.Indexed, o => o.Indexed);

            if (definition.AllowDeletion.HasValue)
                assert.ShouldBeEqual(m => m.AllowDeletion, o => o.AllowDeletion);
            else
                assert.SkipProperty(m => m.AllowDeletion, "AllowDeletion is NULL");


            /// localization
            if (definition.TitleResource.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                 {
                     var srcProp = s.GetExpressionValue(def => def.TitleResource);
                     var isValid = true;

                     foreach (var userResource in s.TitleResource)
                     {
                         var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                         var value = d.TitleResource.GetValueForUICulture(culture);

                         isValid = userResource.Value == value;

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
                         var value = d.DescriptionResource.GetValueForUICulture(culture);

                         isValid = userResource.Value == value;

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

            assert.SkipProperty(m => m.PushChangesToLists,
                "Covered by 'Regression.Scenarios.Fields.PushChangesToLists' test category");
        }
    }
}
