using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.Standard.ModelHosts
{
    public class TermStoreModelHost : SSOMModelHostBase
    {
        #region properties

        public TermStore HostTermStore { get; set; }

        #endregion
    }
}
