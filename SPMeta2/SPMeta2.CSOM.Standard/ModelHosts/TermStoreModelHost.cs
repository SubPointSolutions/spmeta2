using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.CSOM.Standard.ModelHosts
{
    public class TermStoreModelHost : CSOMModelHostBase
    {
        #region properties

        public TermStore HostTermStore { get; set; }

        #endregion
    }
}
