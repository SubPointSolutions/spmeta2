using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.ModelHosts
{
    public class ContentTypeModelHost : WebModelHost
    {
        #region properties

        public ContentType HostContentType { get; set; }

        #endregion
    }
}
