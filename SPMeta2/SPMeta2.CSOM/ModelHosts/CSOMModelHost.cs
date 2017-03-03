using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.ModelHosts;

namespace SPMeta2.CSOM.ModelHosts
{
    public abstract class CSOMModelHostBase : ModelHostBase
    {
        #region constructors

        public CSOMModelHostBase()
        {
            IsCSOM = true;
            IsSSOM = false;
        }

        #endregion

        #region properties

        public ClientContext HostClientContext { get; set; }

        public Site HostSite { get; set; }
        public Web HostWeb { get; set; }

        #endregion
    }
}
