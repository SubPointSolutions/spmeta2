using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.SSOM.ModelHosts;

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
        }
    }
}
