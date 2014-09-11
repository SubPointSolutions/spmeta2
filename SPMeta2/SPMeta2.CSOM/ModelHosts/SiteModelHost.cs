using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{
    public class SiteModelHost : CSOMModelHostBase
    {
        #region constructors

        public SiteModelHost()
        {
            
        }

        public SiteModelHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;
            
            HostSite = clientContext.Site;
            HostWeb = clientContext.Web;
        }

        #endregion

        #region static

        public static SiteModelHost FromClientContext(ClientContext clientContext)
        {
            return new SiteModelHost(clientContext);
        }

        #endregion
    }
}
