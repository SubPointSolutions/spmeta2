using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ModuleFileDefinitionValidator : ModuleFileModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            var folder = folderHost.CurrentLibraryFolder;
            var spObject = GetFile(folderHost, definition);

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     .ShouldBeEqual(m => m.FileName, o => o.Name)
                                     .ShouldBeEqual(m => m.Content, o => o.GetContent());
        }
    }

    internal static class SPFIleExtensions
    {
        public static byte[] GetContent(this SPFile file)
        {
            byte[] result = null;

            using (var stream = file.OpenBinaryStream())
                result = ModuleFileUtils.ReadFully(stream);

            return result;
        }
    }
}
