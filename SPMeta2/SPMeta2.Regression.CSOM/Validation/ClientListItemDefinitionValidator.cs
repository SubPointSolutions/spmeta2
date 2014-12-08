using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            List list = null;
            Folder rootFolder = null;

            if (modelHost is ListModelHost)
            {
                list = (modelHost as ListModelHost).HostList;
                rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }
            }
            else if (modelHost is FolderModelHost)
            {
                list = (modelHost as FolderModelHost).CurrentList;
                rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }
            }

            var spObject = GetListItem(list, rootFolder, definition);

            if (!spObject.IsPropertyAvailable(BuiltInInternalFieldNames.Title))
            {
                var context = spObject.Context;

                context.Load(spObject, o => o.DisplayName);
                context.ExecuteQuery();
            }

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject)
                                   .ShouldBeEqual(m => m.Title, o => o.DisplayName);
        }
    }
}
