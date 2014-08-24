using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListFieldLinkModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListFieldLinkDefinition); }
        }
 
        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listFieldLinkModel = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployListFieldLink(modelHost, list, listFieldLinkModel);
        }

        private void DeployListFieldLink(object modelHost, List list, ListFieldLinkDefinition listFieldLinkModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
