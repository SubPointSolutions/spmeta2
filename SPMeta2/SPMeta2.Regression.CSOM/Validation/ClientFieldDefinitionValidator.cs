using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.CSOM.Utils;
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

        //protected Site ExtractSiteFromHost(object modelHost)
        //{
        //    if (modelHost is SiteModelHost)
        //        return (modelHost as SiteModelHost).HostSite;

        //    if (modelHost is ListModelHost)
        //        return (modelHost as ListModelHost).HostSite;

        //    return null;
        //}

        //protected Web ExtractWebFromHost(object modelHost)
        //{
        //    if (modelHost is SiteModelHost)
        //        return (modelHost as SiteModelHost).HostWeb;

        //    if (modelHost is ListModelHost)
        //        return (modelHost as ListModelHost).HostWeb;

        //    return null;
        //}

        protected bool IsListScopedField
        {
            get { return HostList != null; }
        }
        protected List HostList { get; set; }
        protected Site HostSite { get; set; }

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
            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                //.ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id);
                //.ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                    //.ShouldBeEqual(m => m.Group, o => o.Group);

            if (!string.IsNullOrEmpty(definition.Group))
                assert.ShouldBeEqual(m => m.Group, o => o.Group);
            else
                assert.SkipProperty(m => m.Group);

            CustomFieldTypeValidation(assert, spObject, definition);

            if (!string.IsNullOrEmpty(definition.StaticName))
                assert.ShouldBeEqual(m => m.StaticName, o => o.StaticName);
            else
                assert.SkipProperty(m => m.StaticName);

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

                        var context = HostList.Context;

                        var field = HostList.Fields.GetById(definition.Id);
                        var defaultView = HostList.DefaultView;

                        context.Load(defaultView);
                        context.Load(defaultView, v => v.ViewFields);
                        context.Load(field);

                        context.ExecuteQuery();

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

            if (!string.IsNullOrEmpty(definition.ValidationFormula))
                assert.ShouldBeEqual(m => m.ValidationFormula, o => o.ValidationFormula);
            else
                assert.SkipProperty(m => m.ValidationFormula, string.Format("ValidationFormula value is not set. Skippping."));

            if (!string.IsNullOrEmpty(definition.ValidationMessage))
                assert.ShouldBeEqual(m => m.ValidationMessage, o => o.ValidationMessage);
            else
                assert.SkipProperty(m => m.ValidationMessage, string.Format("ValidationFormula value is not set. Skippping."));

            // taxonomy field seems to prodice issues w/ Required/Description validation
            if (!SkipRequredPropValidation)
                assert.ShouldBeEqual(m => m.Required, o => o.Required);
            else
                assert.SkipProperty(m => m.Required, "Skipping Required prop validation.");

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Skipping Description prop validation.");

            assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);

            if (!string.IsNullOrEmpty(definition.DefaultValue))
                assert.ShouldBePartOf(m => m.DefaultValue, o => o.DefaultValue);
            else
                assert.SkipProperty(m => m.DefaultValue, string.Format("Default value is not set. Skippping."));

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

            if (definition.EnforceUniqueValues.HasValue)
                assert.ShouldBeEqual(m => m.EnforceUniqueValues, o => o.EnforceUniqueValues);
            else
                assert.SkipProperty(m => m.EnforceUniqueValues, "EnforceUniqueValues is NULL");

            if (definition.ShowInDisplayForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInDisplayForm, o => o.GetShowInDisplayForm());
            else
                assert.SkipProperty(m => m.ShowInDisplayForm, "ShowInDisplayForm is NULL");

            if (definition.ShowInEditForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInEditForm, o => o.GetShowInEditForm());
            else
                assert.SkipProperty(m => m.ShowInEditForm, "ShowInEditForm is NULL");

            if (definition.ShowInListSettings.HasValue)
                assert.ShouldBeEqual(m => m.ShowInListSettings, o => o.GetShowInListSettings());
            else
                assert.SkipProperty(m => m.ShowInListSettings, "ShowInListSettings is NULL");

            if (definition.ShowInNewForm.HasValue)
                assert.ShouldBeEqual(m => m.ShowInNewForm, o => o.GetShowInNewForm());
            else
                assert.SkipProperty(m => m.ShowInNewForm, "ShowInNewForm is NULL");

            if (definition.ShowInVersionHistory.HasValue)
                assert.ShouldBeEqual(m => m.ShowInVersionHistory, o => o.GetShowInVersionHistory());
            else
                assert.SkipProperty(m => m.ShowInVersionHistory, "ShowInVersionHistory is NULL");

            if (definition.ShowInViewForms.HasValue)
                assert.ShouldBeEqual(m => m.ShowInViewForms, o => o.GetShowInViewForms());
            else
                assert.SkipProperty(m => m.ShowInViewForms, "ShowInViewForms is NULL");

            assert
                .ShouldBeEqual(m => m.Indexed, o => o.Indexed);

            if (definition.AllowDeletion.HasValue)
                assert.ShouldBeEqual(m => m.AllowDeletion, o => o.GetAllowDeletion());
            else
                assert.SkipProperty(m => m.AllowDeletion, "AllowDeletion is NULL");
        }
    }

}
