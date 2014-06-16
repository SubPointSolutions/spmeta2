using Microsoft.SharePoint.Client;
using SPMeta2.ModelHosts;

namespace SPMeta2.O365.ModelHosts
{
    public abstract class O365ModelHostBase : ModelHostBase
    {
        #region properties

        public ClientContext HostClientContext { get; set; }

        public Site HostSite { get; set; }
        public Web HostWeb { get; set; }

        #endregion
    }
}
