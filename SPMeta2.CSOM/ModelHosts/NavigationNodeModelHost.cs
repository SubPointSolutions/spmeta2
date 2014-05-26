using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{
    public class NavigationNodeModelHost : CSOMModelHostBase
    {
        public NavigationNode HostNavigationNode { get; set; }
    }
}
