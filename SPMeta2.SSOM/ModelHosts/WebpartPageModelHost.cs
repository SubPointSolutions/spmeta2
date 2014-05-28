using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.SSOM.ModelHosts
{
    public class WebpartPageModelHost
    {
        #region properties

        public SPLimitedWebPartManager SPLimitedWebPartManager { get; set; }
        public SPListItem PageListItem { get; set; }

        #endregion
    }
}
