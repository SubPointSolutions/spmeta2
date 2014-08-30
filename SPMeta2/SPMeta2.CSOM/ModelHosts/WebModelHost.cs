using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{
    public class WebModelHost : CSOMModelHostBase
    {
        #region constructors

        public WebModelHost()
        {
            
        }

        public WebModelHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;
            
            HostWeb = clientContext.Web;
            HostSite = clientContext.Site;
        }

        #endregion

        #region static

        public static WebModelHost FromClientContext(ClientContext clientContext)
        {
            return new WebModelHost(clientContext);
        }

        #endregion
    }
}
