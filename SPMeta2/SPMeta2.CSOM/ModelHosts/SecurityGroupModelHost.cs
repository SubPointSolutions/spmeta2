using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHosts
{
    public class SecurityGroupModelHost : CSOMModelHostBase
    {
        #region properties

        public Group SecurityGroup { get; set; }
        public SecurableObject SecurableObject { get; set; }

        #endregion
    }
}
