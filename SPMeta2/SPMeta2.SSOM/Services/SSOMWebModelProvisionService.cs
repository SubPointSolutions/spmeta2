using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.Services
{
    //public class SSOMWebModelProvisionService : ModelProvisionServiceBase
    //{
    //    #region methods

    //    protected override void InternalDeploySiteModel(ProvisionServiceContext context)
    //    {
    //        var site = context.Context as SPSite;
    //        var model = context.Model;

    //        ModelService.DeployModel(SiteModelHost.FromSite(site), model);
    //    }

    //    protected override void InternalDeployRootWebModel(ProvisionServiceContext context)
    //    {
    //        var web = context.Context as SPWeb;
    //        var model = context.Model;

    //        ModelService.DeployModel(WebModelHost.FromWeb(web), model);
    //    }

    //    protected override void InternalDeployWebModel(ProvisionServiceContext context)
    //    {
    //        var web = context.Context as SPWeb;
    //        var model = context.Model;

    //        ModelService.DeployModel(WebModelHost.FromWeb(web), model);
    //    }

    //    #endregion
    //}
}
