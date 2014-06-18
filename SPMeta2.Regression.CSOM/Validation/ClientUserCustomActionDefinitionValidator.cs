using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientUserCustomActionDefinitionValidator : UserCustomActionModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            if (!IsValidHostModelHost(modelHost))
                throw new Exception(string.Format("modelHost of type {0} is not supported.", modelHost.GetType()));

            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var customActionModel = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());

            ValidateModel(siteModelHost, customActionModel);
        }

        private void ValidateModel(SiteModelHost siteModelHost, UserCustomActionDefinition customActionModel)
        {
            var customAction = GetCustomAction(siteModelHost, customActionModel);

            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<UserCustomActionDefinition, UserCustomAction>(customActionModel, customAction);

                traceScope.WriteLine(string.Format("Validating model:[{0}] custom action:[{1}]", customActionModel, customAction));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, o => o.Title)
                    .ShouldBeEqual(trace, m => m.Description, o => o.Description)
                    .ShouldBeEqual(trace, m => m.Group, o => o.Group)
                    .ShouldBeEqual(trace, m => m.Name, o => o.Name)
                    .ShouldBeEqual(trace, m => m.Sequence, o => o.Sequence)
                    .ShouldBeEqual(trace, m => m.Location, o => o.Location));
            });
        }
    }
}
