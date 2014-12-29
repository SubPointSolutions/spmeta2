using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class SecureStoreApplicationModelHost : FarmModelHost
    {
        public ISecureStore HostSecureStore { get; set; }
    }
}
