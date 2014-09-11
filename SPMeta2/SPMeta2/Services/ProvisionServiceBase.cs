using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    //public class ProvisionServiceContext
    //{
    //    public ModelNode Model { get; set; }
    //    public ModelServiceBase ModelService { get; set; }

    //    public object Context { get; set; }
    //}

    //public delegate void ProvisioningServiceEventHandler(ProvisionServiceContext context);

    ///// <summary>
    ///// Internal usage only.
    ///// </summary>
    //public abstract class ModelProvisionServiceBase
    //{
    //    #region properties

    //    public WebModel Model { get; set; }
    //    public ModelServiceBase ModelService { get; set; }

    //    #endregion

    //    #region events

    //    public event ProvisioningServiceEventHandler OnBeforeProvisionSite;
    //    public event ProvisioningServiceEventHandler OnAfterProvisionSite;

    //    public event ProvisioningServiceEventHandler OnBeforeProvisionRootWeb;
    //    public event ProvisioningServiceEventHandler OnAfterProvisionRootWeb;

    //    public event ProvisioningServiceEventHandler OnBeforeProvisionWeb;
    //    public event ProvisioningServiceEventHandler OnAfterProvisionWeb;

    //    #endregion

    //    #region methods

    //    public virtual void ProvisionSite(object context)
    //    {
    //        var model = Model.GetSiteModel();

    //        if (model == null)
    //            return;

    //        var provisionContext = GetProvisionContext(context, model);

    //        WithEvents(OnBeforeProvisionSite, OnAfterProvisionSite, provisionContext, () => InternalDeploySiteModel(provisionContext));
    //    }

    //    public virtual void ProvisionRootWeb(object context)
    //    {
    //        var model = Model.GetRootWebModel();

    //        if (model == null)
    //            return;

    //        var provisionContext = GetProvisionContext(context, model);

    //        WithEvents(OnBeforeProvisionRootWeb, OnAfterProvisionRootWeb, provisionContext, () => InternalDeployRootWebModel(provisionContext));
    //    }

    //    public virtual void ProvisionWeb(object context)
    //    {
    //        var model = Model.GetWebModel();

    //        if (model == null)
    //            return;

    //        var provisionContext = GetProvisionContext(context, model);

    //        WithEvents(OnBeforeProvisionWeb, OnAfterProvisionWeb, provisionContext, () => InternalDeployWebModel(provisionContext));
    //    }

    //    protected abstract void InternalDeploySiteModel(ProvisionServiceContext context);
    //    protected abstract void InternalDeployRootWebModel(ProvisionServiceContext context);
    //    protected abstract void InternalDeployWebModel(ProvisionServiceContext context);

    //    #region utils

    //    private ProvisionServiceContext GetProvisionContext(object context, ModelNode model)
    //    {
    //        return new ProvisionServiceContext
    //        {
    //            Context = context,
    //            Model = model,
    //            ModelService = ModelService
    //        };
    //    }

    //    private static void WithEvents(ProvisioningServiceEventHandler before, ProvisioningServiceEventHandler after,
    //        ProvisionServiceContext context,
    //        Action action)
    //    {
    //        if (before != null)
    //            before(context);

    //        action();

    //        if (after != null)
    //            after(context);
    //    }

    //    #endregion

    //    #endregion
    //}
}
