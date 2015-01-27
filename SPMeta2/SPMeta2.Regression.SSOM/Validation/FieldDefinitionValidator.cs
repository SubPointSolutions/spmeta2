using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Xml.Linq;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);
        }

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
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                //.ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                    .ShouldBeEqual(m => m.Group, o => o.Group);

            CustomFieldTypeValidation(assert, spObject, definition);

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

            assert.ShouldBeEqual(m => m.ValidationFormula, o => o.ValidationFormula);
            assert.ShouldBeEqual(m => m.ValidationMessage, o => o.ValidationMessage);

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
        }
    }
}
