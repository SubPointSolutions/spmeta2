using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class SiteModelHost : SSOMModelHostBase
    {
        #region constructors

        public SiteModelHost()
        {

        }

        public SiteModelHost(SPSite site)
        {
            HostSite = site;
        }

        #endregion

        #region properties

        public SPSite HostSite { get; set; }

        #endregion

        #region static

        public static SiteModelHost FromSite(SPSite site)
        {
            return new SiteModelHost(site);
        }

        #endregion
    }
}
