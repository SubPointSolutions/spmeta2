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
using SPMeta2.SSOM.ModelHandlers.Base;


namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPropertyModelHandler : PropertyModelHandler
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
            var propertyModel = model.WithAssertAndCast<WebPropertyDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;

            DeployProperty(modelHost, web.AllProperties, propertyModel);
        }

        #endregion
    }
}
