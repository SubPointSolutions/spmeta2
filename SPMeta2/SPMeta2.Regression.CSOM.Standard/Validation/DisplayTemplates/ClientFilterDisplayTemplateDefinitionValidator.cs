using System.Collections.Generic;
using System.Linq;
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
    public class FilterDisplayTemplateDefinitionValidator : ItemControlTemplateDefinitionBaseValidator
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<FilterDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);



            if (!string.IsNullOrEmpty(definition.CompatibleManagedProperties))
                assert.ShouldBeEqual(m => m.CompatibleManagedProperties, o => o.GetCompatibleManagedProperties());
            else
                assert.SkipProperty(m => m.CompatibleManagedProperties);

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
                assert.SkipProperty(m => m.CrawlerXSLFileURL, "PreviewURL is NULL. Skipping");
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


            if (definition.CompatibleSearchDataTypes.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CompatibleSearchDataTypes);
                    var isValid = true;

                    var targetValues = (d["CompatibleSearchDataTypes"] as string[])
                                     .Select(v => v.ToUpper()).ToList();

                    foreach (var v in s.CompatibleSearchDataTypes)
                    {
                        if (!targetValues.Contains(v.ToUpper()))
                            isValid = false;
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
                assert.SkipProperty(m => m.CompatibleSearchDataTypes, "CompatibleSearchDataTypes count is 0. Skipping");
            }
        }

        #endregion

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {

        }

        public override System.Type TargetType
        {
            get { return typeof(FilterDisplayTemplateDefinition); }
        }
    }
}
