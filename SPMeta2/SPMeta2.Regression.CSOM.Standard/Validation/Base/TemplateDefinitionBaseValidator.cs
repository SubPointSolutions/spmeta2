using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Base
{
    public abstract class TemplateDefinitionBaseValidator : TemplateModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<TemplateDefinitionBase>("model", value => value.RequireNotNull());

            var spFile = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = spFile.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spFile, f => f.ServerRelativeUrl);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.Title, o => o.GetTitle())
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName())

                                             //.ShouldBeEqual(m => m.CrawlerXSLFile, o => o.GetCrawlerXSLFile())
                                             .ShouldBeEqual(m => m.HiddenTemplate, o => o.GetHiddenTemplate())
                //.ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription())
                                             ;

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription());
            else
                assert.SkipProperty(m => m.Description, "Description is null. Skiping.");

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                isContentValid = dstStringContent.Contains(srcStringContent);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });
        }

        public override string FileExtension
        {
            get { return string.Empty; }
            set
            {

            }
        }


    }
}
