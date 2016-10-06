using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class DocumentSetDefinitionValidator : DocumentSetModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DocumentSetDefinition>("model", value => value.RequireNotNull());

            Folder folder = null;

            if (typedModelHost is ListModelHost)
            {
                folder = (typedModelHost as ListModelHost).HostList.RootFolder;
            }
            else if (typedModelHost is FolderModelHost)
            {
                folder = (typedModelHost as FolderModelHost).CurrentListFolder;
            }

            var context = typedModelHost.HostClientContext;
            var spObject = GetExistingDocumentSet(typedModelHost, folder, definition);

            context.Load(spObject);
            context.Load(spObject, o => o.Properties);
            context.Load(spObject, o => o.ListItemAllFields);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name);

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ContentTypeId);

                    var isValid = ConvertUtils.ToString(d.ListItemAllFields["ContentTypeId"])
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
                    var contenType = GetContentType(typedModelHost, definition);
                    var srcProp = s.GetExpressionValue(m => m.ContentTypeName);

                    var isValid = ConvertUtils.ToString(d.ListItemAllFields["ContentTypeId"])
                                    .StartsWith(contenType.StringId);

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
                    var contenType = GetContentType(typedModelHost, definition);
                    var srcProp = s.GetExpressionValue(m => m.Description);

                    var isValid = ConvertUtils.ToString(d.ListItemAllFields["DocumentSetDescription"])
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

        #endregion
    }
}
