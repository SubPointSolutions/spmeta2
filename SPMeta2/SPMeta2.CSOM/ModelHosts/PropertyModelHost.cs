using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.ModelHosts
{
    public class PropertyModelHost : CSOMModelHostBase
    {
        public Site CurrentSite { get; set; }
        public Web CurrentWeb { get; set; }

        public List CurrentList { get; set; }
        public Folder CurrentFolder { get; set; }

        public ListItem CurrentListItem { get; set; }
        public File CurrentFile { get; set; }
    }
}
