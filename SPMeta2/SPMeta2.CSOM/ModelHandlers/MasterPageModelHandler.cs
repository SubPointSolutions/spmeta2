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
    public class MasterPageModelHandler : MasterPageModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MasterPageDefinition); }
        }

        public override string PageContentTypeId
        {
            get { return BuiltInContentTypeId.MasterPage; }
            set { }
        }

        public override string PageFileExtension
        {
            get { return ".master"; }
            set { }
        }

        #endregion
    }
}
