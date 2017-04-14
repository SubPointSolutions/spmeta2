using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{


    public class ContentTypeLinkModelHost : ContentTypeModelHost
    {
        #region constructors


        #endregion

        #region properties

        public List HostList { get; set; }

        #endregion
    }
}
