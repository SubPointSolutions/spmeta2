using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class ContentTypeModelHost : SSOMModelHostBase
    {
        #region constructors


        #endregion

        #region properties

        public SPContentType HostContentType { get; set; }

        #endregion



        public SPWeb HostWeb { get; set; }
    }
}
