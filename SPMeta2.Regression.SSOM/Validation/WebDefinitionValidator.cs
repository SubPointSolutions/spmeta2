using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class WebDefinitionValidator : WebModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var web = modelHost as SPWeb;

            if (web == null) throw new ArgumentException("modelHost");
        }
    }
}
