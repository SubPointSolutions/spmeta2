using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System.Linq;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientModuleFileDefinitionaValidator : ModuleFileModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            var stringCustomContentType = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
                stringCustomContentType = ResolveContentTypeId(folderHost, definition);

            var folder = folderHost.CurrentLibraryFolder;
            var spObject = GetFile(folderHost, definition);

            if (folderHost.CurrentList != null)
            {
                if (!spObject.IsObjectPropertyInstantiated("ListItemAllFields"))
                    spObject.Context.Load(spObject, o => o.ListItemAllFields);
            }

            if (!spObject.IsObjectPropertyInstantiated("Name"))
                spObject.Context.Load(spObject, o => o.Name);

            if (!spObject.IsObjectPropertyInstantiated("ServerRelativeUrl"))
                spObject.Context.Load(spObject, o => o.ServerRelativeUrl);


            spObject.Context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     .ShouldBeEqual(m => m.FileName, o => o.Name);

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {

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
                    var currentContentTypeName = d.ListItemAllFields["ContentTypeId"].ToString();

                    var isValis = stringCustomContentType == currentContentTypeName;

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

            if (definition.DefaultValues.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    foreach (var srcValue in s.DefaultValues)
                    {
                        // big TODO here for == != 

                        if (!string.IsNullOrEmpty(srcValue.FieldName))
                        {
                            if (d.ListItemAllFields[srcValue.FieldName].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }

                        if (!isValid)
                            break;
                    }

                    var srcProp = s.GetExpressionValue(def => def.DefaultValues);

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
                assert.SkipProperty(m => m.DefaultValues, "DefaultValues.Count == 0. Skipping.");
            }

            // skip all templates
            if (definition.FileName.ToUpper().EndsWith("DOTX"))
            {
                assert.SkipProperty(m => m.Content, "DOTX file is detected. Skipping.");
            }
            else
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Content);
                    //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                    var isContentValid = false;

                    byte[] dstContent = null;

                    using (var stream = File.OpenBinaryDirect(folderHost.HostClientContext, spObject.ServerRelativeUrl).Stream)
                        dstContent = ModuleFileUtils.ReadFully(stream);

                    isContentValid = dstContent.SequenceEqual(definition.Content);

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
    }
}
