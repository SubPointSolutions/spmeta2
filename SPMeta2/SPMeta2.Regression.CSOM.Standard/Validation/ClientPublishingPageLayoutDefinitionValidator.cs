using System;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPublishingPage(folderModelHost.CurrentList, folder, definition);
            var spFile = spObject.File;

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);

            context.Load(spFile, f => f.ServerRelativeUrl);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                             .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageLayoutDescription())
                //.ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId())
                                             .ShouldBeEqual(m => m.Title, o => o.GetTitle());

            if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
                assert.ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId());
            else
                assert.SkipProperty(m => m.AssociatedContentTypeId, "AssociatedContentTypeId is null or empty.");

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = s.Content;
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                srcStringContent = srcStringContent
                   .Replace("meta:webpartpageexpansion=\"full\" ", string.Empty);

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

        #endregion
    }



    internal static class PublishingPageLayoutItemHelper
    {
        public static string GetPublishingPageLayoutDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
