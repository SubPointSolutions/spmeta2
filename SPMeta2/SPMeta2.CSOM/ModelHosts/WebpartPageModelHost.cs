using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;

namespace SPMeta2.CSOM.ModelHosts
{

    public class WebpartPageModelHost : CSOMModelHostBase
    {
        #region properties

        public LimitedWebPartManager SPLimitedWebPartManager { get; set; }
        public ListItem PageListItem { get; set; }

        #endregion
    }
}
