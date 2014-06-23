using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientQuickLaunchNavigationNodeDefinitionValidator : QuickLaunchNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var quickLaunchModel = model.WithAssertAndCast<QuickLaunchNavigationNodeDefinition>("model", value => value.RequireNotNull());

            if (ShouldDeployRootNavigationNode(modelHost))
                ValidateRootQuickLaunchNavigationNode(modelHost as WebModelHost, quickLaunchModel);
            else if (ShouldDeployNavigationNode(modelHost))
                ValidateQuickLaunchNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchModel);
            else
                throw new ArgumentException("modelHost needs to be WebModelHost or NavigationNodeModelHost");
        }

        private void ValidateQuickLaunchNavigationNode(NavigationNodeModelHost navigationNodeModelHost, QuickLaunchNavigationNodeDefinition quickLaunchModel)
        {
            var qlNode = GetNavigationNode(navigationNodeModelHost, quickLaunchModel);

            ValidateNode(qlNode, quickLaunchModel);
        }

        private void ValidateRootQuickLaunchNavigationNode(WebModelHost webModelHost, QuickLaunchNavigationNodeDefinition quickLaunchModel)
        {
            var qlNode = GetRootNavigationNode(webModelHost, quickLaunchModel);

            ValidateNode(qlNode, quickLaunchModel);
        }

        private void ValidateNode(NavigationNode qlNode, QuickLaunchNavigationNodeDefinition quickLaunchModel)
        {
            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<QuickLaunchNavigationNodeDefinition, NavigationNode>(quickLaunchModel, qlNode);

                traceScope.WriteLine(string.Format("Validating model:[{0}] node:[{1}]", quickLaunchModel, qlNode));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, o => o.Title)
                    .ShouldBeEqual(trace, m => m.Url, o => o.Url)
                    .ShouldBeEqual(trace, m => m.IsVisible, o => o.IsVisible)
                    .ShouldBeEqual(trace, m => m.IsExternal, o => o.IsExternal));
            });
        }
    }
}
