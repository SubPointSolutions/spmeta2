using Microsoft.SharePoint.Client.Taxonomy;

namespace SPMeta2.CSOM.Standard.ModelHosts
{
    public class TermModelHost : TermSetModelHost
    {
        #region properties

        public Term HostTerm { get; set; }

        #endregion
    }
}
