﻿using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{
    public class ListModelHost : WebModelHost
    {
        #region properties

        public List HostList { get; set; }

        #endregion
    }
}
