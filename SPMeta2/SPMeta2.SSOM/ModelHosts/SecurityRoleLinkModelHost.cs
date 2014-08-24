using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class SecurityGroupLinkModel : SSOMModelHostBase
    {
        #region properties

        public SPGroup SecurityGroup { get; set; }
        public SPSecurableObject SecurableObject { get; set; }

        #endregion
    }
}
