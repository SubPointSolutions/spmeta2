using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFolderDefinitionValidator : FolderModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            Folder spObject = null;

            if (ShouldDeployLibraryFolder(folderModelHost))
                spObject = GetLibraryFolder(folderModelHost, definition);
            else if (ShouldDeployListFolder(folderModelHost))
                spObject = GetListFolder(folderModelHost, definition);

            var context = spObject.Context;

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject);

            assert.ShouldBeEqual(m => m.Name, o => o.Name);

            context.Load(spObject);
            context.Load(spObject, o => o.ListItemAllFields);
            context.ExecuteQueryWithTrace();

            var item = spObject.ListItemAllFields;

            var stringCustomContentType = string.Empty;
            var stringCustomContentTypeId = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                if (!string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    var ct = ContentTypeLookupService
                                                    .LookupContentTypeByName(item.ParentList, definition.ContentTypeName);

                    stringCustomContentType = ct.Name;
                    stringCustomContentTypeId = ct.StringId;
                }
            }


            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeId);
                    var currentContentTypeId = ConvertUtils.ToString(item["ContentTypeId"]);

                    var isValis = currentContentTypeId.StartsWith(s.ContentTypeId);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValis
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeId, "ContentTypeId is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);

                    // still validating agains content type ID.
                    // setting up by Name, the item must have correct ID
                    var currentContentTypeId = ConvertUtils.ToString(item["ContentTypeId"]);
                    var isValis = currentContentTypeId.StartsWith(stringCustomContentTypeId);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValis
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            }
        }
    }
}
