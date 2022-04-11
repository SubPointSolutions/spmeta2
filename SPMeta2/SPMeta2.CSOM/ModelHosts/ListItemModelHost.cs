using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.ModelHosts
{
    public class ListItemModelHost : CSOMModelHostBase
    {
        #region properties
        public Folder HostFolder { get; set; }

        public ListItem HostListItem { get; set; }
        public File HostFile { get; set; }

        #endregion

        public List HostList { get; set; }

        public bool IsSpecialFolderContext { get; set; }
    }
}
