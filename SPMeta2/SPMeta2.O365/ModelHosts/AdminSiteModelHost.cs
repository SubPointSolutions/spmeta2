using Microsoft.SharePoint.Client;

namespace SPMeta2.O365.ModelHosts
{
    public class AdminSiteModelHost : O365ModelHostBase
    {
        #region constructors

        public AdminSiteModelHost()
        {
            
        }

        public AdminSiteModelHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;
            HostSite = clientContext.Site;
        }

        #endregion

        #region static

        public static AdminSiteModelHost FromClientContext(ClientContext clientContext)
        {
            return new AdminSiteModelHost(clientContext);
        }

        #endregion
    }
}
