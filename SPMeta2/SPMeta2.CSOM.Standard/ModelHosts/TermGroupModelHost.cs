using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace SPMeta2.CSOM.Standard.ModelHosts
{
    public class TermGroupModelHost : TermStoreModelHost
    {
        #region properties

        public TermGroup HostGroup { get; set; }

        #endregion
    }
}
