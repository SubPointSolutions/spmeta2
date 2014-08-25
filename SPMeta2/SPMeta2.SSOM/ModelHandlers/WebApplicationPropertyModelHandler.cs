using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebApplicationPropertyModelHandler : PropertyModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebApplicationPropertyDefinition); }
        }


        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webApplicationModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var propertyModel = model.WithAssertAndCast<WebApplicationPropertyDefinition>("model", value => value.RequireNotNull());

            var webApp = webApplicationModelHost.HostWebApplication;

            DeployProperty(modelHost, webApp.Properties, propertyModel);
        }

        #endregion
    }
}
