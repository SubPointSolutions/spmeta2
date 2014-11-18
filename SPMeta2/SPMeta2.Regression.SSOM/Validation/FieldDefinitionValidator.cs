using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);


            ValidateField(assert, spObject, definition);
        }

        protected void ValidateField(AssertPair<FieldDefinition, SPField> assert, SPField spObject, FieldDefinition definition)
        {
            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                    .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                    .ShouldBeEqual(m => m.Group, o => o.Group);

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
