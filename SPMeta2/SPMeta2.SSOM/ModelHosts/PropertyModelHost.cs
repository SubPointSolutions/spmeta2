using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.SSOM.ModelHosts
{
    public class PropertyModelHost : SSOMModelHostBase
    {
        public SPFarm CurrentFarm { get; set; }
        public SPWebApplication CurrentWebApplication { get; set; }

        public SPSite CurrentSite { get; set; }
        public SPWeb CurrentWeb { get; set; }

        public SPList CurrentList { get; set; }
        public SPFolder CurrentFolder { get; set; }

        public SPListItem CurrentListItem { get; set; }
        public SPFile CurrentFile { get; set; }
    }
}
