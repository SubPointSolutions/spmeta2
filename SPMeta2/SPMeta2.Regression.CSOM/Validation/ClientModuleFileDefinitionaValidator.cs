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

            var folder = folderHost.CurrentLibraryFolder;
            var spObject = GetFile(folderHost, definition);

            if (!spObject.IsObjectPropertyInstantiated("Name"))
                spObject.Context.Load(spObject, o => o.Name);

            if (!spObject.IsObjectPropertyInstantiated("ServerRelativeUrl"))
                spObject.Context.Load(spObject, o => o.ServerRelativeUrl);

            spObject.Context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     .ShouldBeEqual(m => m.FileName, o => o.Name);

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
