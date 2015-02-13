using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.SSOM.ModelHosts
{
    public class WebpartPageModelHost : SSOMModelHostBase
    {
        #region properties

        public SPFile HostFile { get; set; }
        public SPLimitedWebPartManager SPLimitedWebPartManager { get; set; }
        public SPListItem PageListItem { get; set; }

        #endregion
    }
}
