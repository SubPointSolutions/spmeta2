using System;
using Microsoft.SharePoint.Publishing.WebControls;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.Office.Server.WebControls;


namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class AdvancedSearchBoxValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AdvancedSearchBoxDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // content editor specific validation

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var typedModel = model.WithAssertAndCast<AdvancedSearchBoxDefinition>("model", value => value.RequireNotNull());

            //var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(host.HostFile, typedModel, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as AdvancedSearchBox;

                var assert = ServiceFactory.AssertService
                                           .NewAssert(typedModel, typedWebPart)
                                           .ShouldNotBeNull(typedWebPart);


                if (!string.IsNullOrEmpty(typedModel.AndQueryTextBoxLabelText))
                    assert.ShouldBeEqual(m => m.AndQueryTextBoxLabelText, o => o.AndQueryTextBoxLabelText);
                else
                    assert.SkipProperty(m => m.AndQueryTextBoxLabelText);

                if (!string.IsNullOrEmpty(typedModel.DisplayGroup))
                    assert.ShouldBeEqual(m => m.DisplayGroup, o => o.DisplayGroup);
                else
                    assert.SkipProperty(m => m.DisplayGroup);

                if (!string.IsNullOrEmpty(typedModel.LanguagesLabelText))
                    assert.ShouldBeEqual(m => m.LanguagesLabelText, o => o.LanguagesLabelText);
                else
                    assert.SkipProperty(m => m.LanguagesLabelText);

                if (!string.IsNullOrEmpty(typedModel.NotQueryTextBoxLabelText))
                    assert.ShouldBeEqual(m => m.NotQueryTextBoxLabelText, o => o.NotQueryTextBoxLabelText);
                else
                    assert.SkipProperty(m => m.NotQueryTextBoxLabelText);

                if (!string.IsNullOrEmpty(typedModel.OrQueryTextBoxLabelText))
                    assert.ShouldBeEqual(m => m.OrQueryTextBoxLabelText, o => o.OrQueryTextBoxLabelText);
                else
                    assert.SkipProperty(m => m.OrQueryTextBoxLabelText);

                if (!string.IsNullOrEmpty(typedModel.PhraseQueryTextBoxLabelText))
                    assert.ShouldBeEqual(m => m.PhraseQueryTextBoxLabelText, o => o.PhraseQueryTextBoxLabelText);
                else
                    assert.SkipProperty(m => m.PhraseQueryTextBoxLabelText);

                if (!string.IsNullOrEmpty(typedModel.AdvancedSearchBoxProperties))
                    assert.ShouldBeEqual(m => m.AdvancedSearchBoxProperties, o => o.Properties);
                else
                    assert.SkipProperty(m => m.AdvancedSearchBoxProperties);

                if (!string.IsNullOrEmpty(typedModel.PropertiesSectionLabelText))
                    assert.ShouldBeEqual(m => m.PropertiesSectionLabelText, o => o.PropertiesSectionLabelText);
                else
                    assert.SkipProperty(m => m.PropertiesSectionLabelText);

                if (!string.IsNullOrEmpty(typedModel.ResultTypeLabelText))
                    assert.ShouldBeEqual(m => m.ResultTypeLabelText, o => o.ResultTypeLabelText);
                else
                    assert.SkipProperty(m => m.ResultTypeLabelText);

                if (!string.IsNullOrEmpty(typedModel.ScopeLabelText))
                    assert.ShouldBeEqual(m => m.ScopeLabelText, o => o.ScopeLabelText);
                else
                    assert.SkipProperty(m => m.ScopeLabelText);

                if (!string.IsNullOrEmpty(typedModel.ScopeSectionLabelText))
                    assert.ShouldBeEqual(m => m.ScopeSectionLabelText, o => o.ScopeSectionLabelText);
                else
                    assert.SkipProperty(m => m.ScopeSectionLabelText);

                if (!string.IsNullOrEmpty(typedModel.SearchResultPageURL))
                    assert.ShouldBeEqual(m => m.SearchResultPageURL, o => o.SearchResultPageURL);
                else
                    assert.SkipProperty(m => m.SearchResultPageURL);

                if (typedModel.ShowAndQueryTextBox.HasValue)
                    assert.ShouldBeEqual(m => m.ShowAndQueryTextBox, o => o.ShowAndQueryTextBox);
                else
                    assert.SkipProperty(m => m.ShowAndQueryTextBox);

                if (typedModel.ShowLanguageOptions.HasValue)
                    assert.ShouldBeEqual(m => m.ShowLanguageOptions, o => o.ShowLanguageOptions);
                else
                    assert.SkipProperty(m => m.ShowLanguageOptions);

                if (typedModel.ShowNotQueryTextBox.HasValue)
                    assert.ShouldBeEqual(m => m.ShowNotQueryTextBox, o => o.ShowNotQueryTextBox);
                else
                    assert.SkipProperty(m => m.ShowNotQueryTextBox);

                if (typedModel.ShowOrQueryTextBox.HasValue)
                    assert.ShouldBeEqual(m => m.ShowOrQueryTextBox, o => o.ShowOrQueryTextBox);
                else
                    assert.SkipProperty(m => m.ShowOrQueryTextBox);

                if (typedModel.ShowPhraseQueryTextBox.HasValue)
                    assert.ShouldBeEqual(m => m.ShowPhraseQueryTextBox, o => o.ShowPhraseQueryTextBox);
                else
                    assert.SkipProperty(m => m.ShowPhraseQueryTextBox);

                if (typedModel.ShowPropertiesSection.HasValue)
                    assert.ShouldBeEqual(m => m.ShowPropertiesSection, o => o.ShowPropertiesSection);
                else
                    assert.SkipProperty(m => m.ShowPropertiesSection);

                if (typedModel.ShowResultTypePicker.HasValue)
                    assert.ShouldBeEqual(m => m.ShowResultTypePicker, o => o.ShowResultTypePicker);
                else
                    assert.SkipProperty(m => m.ShowResultTypePicker);

                if (typedModel.ShowScopes.HasValue)
                    assert.ShouldBeEqual(m => m.ShowScopes, o => o.ShowScopes);
                else
                    assert.SkipProperty(m => m.ShowScopes);

                if (!string.IsNullOrEmpty(typedModel.TextQuerySectionLabelText))
                    assert.ShouldBeEqual(m => m.TextQuerySectionLabelText, o => o.TextQuerySectionLabelText);
                else
                    assert.SkipProperty(m => m.TextQuerySectionLabelText);
            });
        }

        #endregion
    }
}
