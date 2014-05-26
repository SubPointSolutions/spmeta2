using System;
using Microsoft.SharePoint.Administration;
using SPDevLab.SPMeta2.Definitions;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FarmModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(FarmDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var farm = modelHost.WithAssertAndCast<SPFarm>("modelHost", value => value.RequireNotNull());
            var farmModel = model.WithAssertAndCast<FarmDefinition>("model", value => value.RequireNotNull());
        }
             

        #endregion
    }
}
