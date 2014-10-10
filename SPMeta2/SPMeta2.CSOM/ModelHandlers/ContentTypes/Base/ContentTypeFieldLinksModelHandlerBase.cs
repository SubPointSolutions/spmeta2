using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes.Base
{
    public abstract class ContentTypeFieldLinksModelHandlerBase : CSOMModelHandlerBase
    {
        #region methods

        protected ContentType ExtractContentTypeFromHost(object host)
        {
            return (host as ModelHostContext).ContentType;
        }

        protected Folder ExtractFolderFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.RootFolder;

            if (modelHost is FolderModelHost)
                return (modelHost as FolderModelHost).CurrentLibraryFolder;

            throw new ArgumentException("modelHost needs to be ListModelHost or FolderModelHost");
        }

        protected List ExtractListFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            if (modelHost is FolderModelHost)
            {
                var host = modelHost as FolderModelHost;

                return host.CurrentList;
            }

            throw new ArgumentException("modelHost needs to be ListModelHost or FolderModelHost");
        }

        #endregion
    }
}
