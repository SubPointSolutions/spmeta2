using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.ModelHosts
{
    public class PropertyModelHost : CSOMModelHostBase
    {
        public Web CurrentWeb { get; set; }
        public List CurrentList { get; set; }
        
        public ListItem CurrentListItem { get; set; }
        public File CurrentFile { get; set; }
    }
}
