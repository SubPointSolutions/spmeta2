using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHosts
{
    public class SecurityGroupLinkModel
    {
        #region properties

        public SPGroup SecurityGroup { get; set; }
        public SPSecurableObject SecurableObject { get; set; }

        #endregion
    }
}
