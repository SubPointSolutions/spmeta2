using System;
using System.Linq;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class DocumentSetDefinitionValidator : DocumentSetModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentSetDefinition>("model", value => value.RequireNotNull());

            SPFolder folder = null;

            if (typedModelHost is ListModelHost)
            {
                folder = (typedModelHost as ListModelHost).HostList.RootFolder;
            }
            else if (typedModelHost is FolderModelHost)
            {
                folder = (typedModelHost as FolderModelHost).CurrentLibraryFolder;
            }

            var web = folder.ParentWeb;
            var spObject = GetExistingDocumentSet(typedModelHost, folder, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name);

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ContentTypeId);

                    var isValid = ConvertUtils.ToString(d.Item["ContentTypeId"])
                                   .StartsWith(s.ContentTypeId);

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
                assert.SkipProperty(m => m.ContentTypeId);
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var contenType = GetContentType(web, definition);
                    var srcProp = s.GetExpressionValue(m => m.ContentTypeName);

                    var isValid = ConvertUtils.ToString(d.Item["ContentTypeId"])
                                    .StartsWith(contenType.Id.ToString());

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
                assert.SkipProperty(m => m.ContentTypeName);
            }

            if (!string.IsNullOrEmpty(definition.Description))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var contenType = GetContentType(web, definition);
                    var srcProp = s.GetExpressionValue(m => m.Description);

                    var isValid = ConvertUtils.ToString(d.Item["DocumentSetDescription"])
                                        == s.Description;

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
                assert.SkipProperty(m => m.Description);
            }

        }
    }
}
