using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;


namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPropertyModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebPropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var webProperty = model.WithAssertAndCast<WebPropertyDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;

            DeploytWebProperty(webModelHost, web, webProperty);
        }

        private void DeploytWebProperty(ModelHosts.WebModelHost webModelHost, Microsoft.SharePoint.SPWeb web, Definitions.WebPropertyDefinition webProperty)
        {
            var currentValue = web.GetProperty(webProperty.Key);

            if (currentValue == null)
            {
                web.SetProperty(webProperty.Key, webProperty.Value);
                web.Update();
            }
            else
            {
                if (webProperty.Overwrite)
                {
                    web.SetProperty(webProperty.Key, webProperty.Value);
                    web.Update();
                }
            }
        }

        #endregion
    }
}
