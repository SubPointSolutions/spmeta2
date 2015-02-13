using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.SSOM.ModelHosts
{
    public class WebApplicationModelHost : SSOMModelHostBase
    {
        #region constructors

        public WebApplicationModelHost()
        {

        }

        public WebApplicationModelHost(SPWebApplication webApp)
        {
            HostWebApplication = webApp;
        }

        #endregion

        #region properties

        public SPWebApplication HostWebApplication { get; set; }

        #endregion

        #region static

        public static WebApplicationModelHost FromWebApplication(SPWebApplication webApp)
        {
            return new WebApplicationModelHost(webApp);
        }

        #endregion
    }
}
