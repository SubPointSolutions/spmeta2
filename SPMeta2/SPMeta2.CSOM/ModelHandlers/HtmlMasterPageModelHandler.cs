using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class HtmlMasterPageModelHandler : MasterPageModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(HtmlMasterPageDefinition); }
        }

        public override string PageContentTypeId
        {
            get { return BuiltInSiteContentTypeId.HtmlMasterPage; }
            set { }
        }

        public override string PageFileExtension
        {
            get { return ".html"; }
            set { }
        }

        #endregion
    }
}
