using System;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class PublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name)
                                             .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())

                                             .ShouldBeEqual(m => m.Title, o => o.Title);

            if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
                assert.ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId());
            else
                assert.SkipProperty(m => m.AssociatedContentTypeId);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                var srcStringContent = s.Content;
                var dstStringContent = Encoding.UTF8.GetString(file.GetContent());

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
    }

    public static class PublishingPageLayoutItemHelper
    {
        public static string GetPublishingPageLayoutDescription(this SPListItem item)
        {
            return item[BuiltInPublishingFieldId.Description] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
