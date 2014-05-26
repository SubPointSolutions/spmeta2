using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SiteModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SiteDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost.WithAssertAndCast<SPSite>("modelHost", value => value.RequireNotNull());
            var siteModel = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());
        }

        #endregion
    }
}
