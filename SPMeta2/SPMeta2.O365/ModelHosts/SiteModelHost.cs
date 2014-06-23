using Microsoft.SharePoint.Client;

namespace SPMeta2.O365.ModelHosts
{
    public class SiteModelHost : O365ModelHostBase
    {
        #region constructors

        public SiteModelHost()
        {
            
        }

        public SiteModelHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;
            HostSite = clientContext.Site;
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
