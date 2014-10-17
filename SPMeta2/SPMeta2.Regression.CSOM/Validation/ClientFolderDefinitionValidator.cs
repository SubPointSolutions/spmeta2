using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;

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

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldBeEqual(m => m.Name, o => o.Name);
        }
    }
}
