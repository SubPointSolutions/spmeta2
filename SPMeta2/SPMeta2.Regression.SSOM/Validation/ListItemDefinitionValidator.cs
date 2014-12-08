using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            SPList list = null;
            SPFolder rootFolder = null;

            if (modelHost is ListModelHost)
            {
                list = (modelHost as ListModelHost).HostList;
                rootFolder = (modelHost as ListModelHost).HostList.RootFolder;
            }
            else if (modelHost is FolderModelHost)
            {
                list = (modelHost as FolderModelHost).CurrentList;
                rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;
            }

            var spObject = GetListItem(list, rootFolder, definition);

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject)
                                   .ShouldBeEqual(m => m.Title, o => o.Title);
        }
    }
}
