using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Services
{
    //public class CSOMWebModelProvisionService : ModelProvisionServiceBase
    //{
    //    #region methods

    //    protected override void InternalDeploySiteModel(ProvisionServiceContext context)
    //    {
    //        var clientContext = context.Context as ClientContext;
    //        var model = context.Model;

    //        ModelService.DeployModel(SiteModelHost.FromClientContext(clientContext), model);
    //    }

    //    protected override void InternalDeployRootWebModel(ProvisionServiceContext context)
    //    {
    //        var clientContext = context.Context as ClientContext;
    //        var model = context.Model;

    //        ModelService.DeployModel(WebModelHost.FromClientContext(clientContext), model);
    //    }

    //    protected override void InternalDeployWebModel(ProvisionServiceContext context)
    //    {
    //        var clientContext = context.Context as ClientContext;
    //        var model = context.Model;

    //        ModelService.DeployModel(WebModelHost.FromClientContext(clientContext), model);
    //    }

    //    #endregion
    //}
}
