using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Standard.Validation.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.DisplayTemplates
{
    public class ClientControlDisplayTemplateDefinitionValidator : ItemControlTemplateDefinitionBaseValidator
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<ControlDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);



            #region crawler xslt file

            if (!string.IsNullOrEmpty(definition.CrawlerXSLFileURL))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;

                    var srcProp = s.GetExpressionValue(m => m.CrawlerXSLFileURL);
                    var crawlerXSLFile = d.GetCrawlerXSLFile();

                    if (crawlerXSLFile != null)
                        isValid = d.GetCrawlerXSLFile().Url == s.CrawlerXSLFileURL;

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
                assert.SkipProperty(m => m.CrawlerXSLFileURL, "CrawlerXSLFileURL is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.CrawlerXSLFileDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;
                    var srcProp = s.GetExpressionValue(m => m.CrawlerXSLFileDescription);

                    var crawlerXSLFile = d.GetCrawlerXSLFile();

                    if (crawlerXSLFile != null)
                        isValid = d.GetCrawlerXSLFile().Description == s.CrawlerXSLFileDescription;

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
                assert.SkipProperty(m => m.CrawlerXSLFileDescription, "PreviewDescription is NULL. Skipping");
            }

            #endregion
        }

        #endregion

        public override string FileExtension
        {
            get { return "html"; }
            set { }
        }

        public override System.Type TargetType
        {
            get { return typeof(ControlDisplayTemplateDefinition); }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {

        }
    }
}
