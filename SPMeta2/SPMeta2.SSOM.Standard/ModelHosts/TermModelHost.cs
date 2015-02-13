using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Taxonomy;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.Standard.ModelHosts
{
    public class TermModelHost : TermSetModelHost
    {
        #region properties

        public Term HostTerm { get; set; }

        #endregion
    }
}
