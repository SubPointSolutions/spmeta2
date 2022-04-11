using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FolderDefinitionValidator : FolderModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            SPFolder spObject = null;

            if (folderModelHost.CurrentLibrary != null)
                spObject = GetLibraryFolder(folderModelHost, definition);
            else if (folderModelHost.CurrentList != null)
                spObject = GetListFolder(folderModelHost, definition);

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldBeEqual(m => m.Name, o => o.Name);

            var item = spObject.Item;

            var stringCustomContentTypeId = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                if (!string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    var ct = ContentTypeLookupService
                                                    .LookupContentTypeByName(item.ParentList, definition.ContentTypeName);

                    stringCustomContentTypeId = ct.ToString();
                }

                if (!string.IsNullOrEmpty(definition.ContentTypeId))
                {
                    var ct = ContentTypeLookupService
                                                    .LookupListContentTypeById(item.ParentList, definition.ContentTypeId);

                    stringCustomContentTypeId = ct.ToString();
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
                    var isValis = stringCustomContentTypeId.StartsWith(currentContentTypeId);

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
