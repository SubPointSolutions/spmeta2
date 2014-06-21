using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class SiteDefinitionValidator : SiteModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
        }
    }
}
