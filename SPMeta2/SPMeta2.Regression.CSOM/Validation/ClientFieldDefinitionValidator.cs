﻿using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Services;
using SPMeta2.Utils;


namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFieldDefinitionValidator : FieldModelHandler
    {
        #region properties

        public bool SkipRequredPropValidation { get; set; }
        public bool SkipDescriptionPropValidation { get; set; }

        #endregion

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            HostList = ExtractListFromHost(modelHost);
            HostSite = ExtractSiteFromHost(modelHost);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);
        }

        protected bool IsListScopedField
        {
            get { return HostList != null; }
        }
        protected List HostList { get; set; }
        // protected Site HostSite { get; set; }

        protected Field GetField(object modelHost, FieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return FindExistingSiteField(modelHost as SiteModelHost, definition);
            if (modelHost is WebModelHost)
                return FindExistingWebField(modelHost as WebModelHost, definition);
            else if (modelHost is ListModelHost)
                return FindExistingListField((modelHost as ListModelHost).HostList, definition);
            else
            {
                throw new SPMeta2NotSupportedException(
                    string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    definition.GetType(),
                    modelHost.GetType()));
            }
        }

        protected virtual void CustomFieldTypeValidation(AssertPair<FieldDefinition, Field> assert, Field spObject,
           FieldDefinition definition)
        {
            assert.ShouldBeEqual(m => m.FieldType, o => o.TypeAsString);
        }

        protected void ValidateField(AssertPair<FieldDefinition, Field> assert, Field spObject, FieldDefinition definition)
        {
            var context = spObject.Context;

            CustomFieldTypeValidation(assert, spObject, definition);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.Id, o => o.Id);

            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.Group, o => o.Group);
            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.StaticName, o => o.StaticName);

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

                        var field = HostList.Fields.GetById(definition.Id);
                        var defaultView = HostList.DefaultView;

                        context.Load(defaultView);
                        context.Load(defaultView, v => v.ViewFields);
                        context.Load(field);

                        context.ExecuteQueryWithTrace();

                        var isValid = HostList.DefaultView
                            .ViewFields
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

            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.ValidationFormula, o => o.ValidationFormula);
            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.ValidationMessage, o => o.ValidationMessage);

            // taxonomy field seems to prodice issues w/ Required/Description validation
            if (!SkipRequredPropValidation)
                assert.ShouldBeEqual(m => m.Required, o => o.Required);
            else
                assert.SkipProperty(m => m.Required);

            assert.ShouldBeEqualIfNotNullOrEmpty(m => m.Description, o => o.Description);
            assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);

            assert.ShouldBePartOfIfNotNullOrEmpty(m => m.DefaultValue, o => o.DefaultValue);

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

            assert.ShouldBeEqualIfHasValue(m => m.EnforceUniqueValues, o => o.EnforceUniqueValues);

            assert.ShouldBeEqualIfHasValue(m => m.ShowInDisplayForm, o => o.GetShowInDisplayForm());
            assert.ShouldBeEqualIfHasValue(m => m.ShowInEditForm, o => o.GetShowInEditForm());
            assert.ShouldBeEqualIfHasValue(m => m.ShowInListSettings, o => o.GetShowInListSettings());
            assert.ShouldBeEqualIfHasValue(m => m.ShowInNewForm, o => o.GetShowInNewForm());
            assert.ShouldBeEqualIfHasValue(m => m.ShowInVersionHistory, o => o.GetShowInVersionHistory());
            assert.ShouldBeEqualIfHasValue(m => m.ShowInViewForms, o => o.GetShowInViewForms());

            assert.ShouldBeEqual(m => m.Indexed, o => o.Indexed);

            assert.ShouldBeEqualIfHasValue(m => m.AllowDeletion, o => o.GetAllowDeletion());

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
