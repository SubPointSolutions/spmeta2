using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes.Base
{
    public abstract class ContentTypeFieldLinksModelHandlerBase : CSOMModelHandlerBase
    {
        #region methods

        protected ContentType ExtractContentTypeFromHost(object host)
        {
            if (host is ContentTypeModelHost)
            {
                return (host as ContentTypeModelHost).HostContentType;
            }
            else if (host is ContentTypeLinkModelHost)
            {
                return (host as ContentTypeLinkModelHost).HostContentType;
            }
            else if (host is ModelHostContext)
            {
                return (host as ModelHostContext).ContentType;
            }

            throw new SPMeta2Exception(
                string.Format("Unsupported model host type:[{0}]",
                host.GetType()));
        }

        protected Folder ExtractFolderFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.RootFolder;

            if (modelHost is FolderModelHost)
                return (modelHost as FolderModelHost).CurrentListFolder;

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
