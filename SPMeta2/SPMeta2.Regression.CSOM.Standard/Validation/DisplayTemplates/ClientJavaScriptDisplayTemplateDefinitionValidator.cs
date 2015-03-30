using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Standard.Validation.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.DisplayTemplates
{
    public class ClientJavaScriptDisplayTemplateDefinitionValidator : TemplateDefinitionBaseValidator
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<JavaScriptDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual(m => m.Standalone, o => o.GetStandalone());
            assert.ShouldBeEqual(m => m.TargetControlType, o => o.GetTargetControlType());
            assert.ShouldBeEqual(m => m.TargetListTemplateId, o => o.GetTargetListTemplateId());
            assert.ShouldBeEqual(m => m.TargetScope, o => o.GetTargetScope());

            #region icon url

            if (!string.IsNullOrEmpty(definition.IconUrl))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IconUrl);
                    var isValid = d.GetDisplayTemplateJSIconUrl().Url == s.IconUrl;

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
                assert.SkipProperty(m => m.IconUrl, "IconUrl is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.IconDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IconDescription);
                    var isValid = d.GetDisplayTemplateJSIconUrl().Description == s.IconDescription;

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
                assert.SkipProperty(m => m.IconDescription, "IconDescription is NULL. Skipping");
            }

            #endregion
        }

        #endregion

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {

        }

        public override System.Type TargetType
        {
            get { return typeof(JavaScriptDisplayTemplateDefinition); }
        }
    }
}
