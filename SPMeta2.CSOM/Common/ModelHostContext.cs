using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Common
{
    public class ModelHostContext
    {
        #region properties

        public Site Site { get; set; }
        public Web Web { get; set; }
        public ContentType ContentType { get; set; }
        public List List { get; set; }
        public Group SecurityGroup { get; set; }

        #endregion
    }
}
