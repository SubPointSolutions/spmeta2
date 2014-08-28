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
    public class ClientTopNavigationNodeDefinitionValidator : TopNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var quickLaunchModel = model.WithAssertAndCast<TopNavigationNodeDefinition>("model", value => value.RequireNotNull());

            if (ShouldDeployRootNavigationNode(modelHost))
                ValidateRootQuickLaunchNavigationNode(modelHost as WebModelHost, quickLaunchModel);
            else if (ShouldDeployNavigationNode(modelHost))
                ValidateQuickLaunchNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchModel);
            else
                throw new ArgumentException("modelHost needs to be WebModelHost or NavigationNodeModelHost");
        }

        private void ValidateQuickLaunchNavigationNode(NavigationNodeModelHost navigationNodeModelHost, TopNavigationNodeDefinition quickLaunchModel)
        {
            var qlNode = GetNavigationNode(navigationNodeModelHost, quickLaunchModel);

            ValidateNode(qlNode, quickLaunchModel);
        }

        private void ValidateRootQuickLaunchNavigationNode(WebModelHost webModelHost, TopNavigationNodeDefinition quickLaunchModel)
        {
            var qlNode = GetRootNavigationNode(webModelHost, quickLaunchModel);

            ValidateNode(qlNode, quickLaunchModel);
        }

        private void ValidateNode(NavigationNode qlNode, TopNavigationNodeDefinition quickLaunchModel)
        {
            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<TopNavigationNodeDefinition, NavigationNode>(quickLaunchModel, qlNode);

                traceScope.WriteLine(string.Format("Validating model:[{0}] node:[{1}]", quickLaunchModel, qlNode));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, o => o.Title)

                    .ShouldBeEqual(trace, m => m.IsVisible, o => o.IsVisible)
                    .ShouldBeEqual(trace, m => m.IsExternal, o => o.IsExternal));

                // should end with
                // TODO
                Assert.IsTrue(qlNode.Url.ToUpper().EndsWith(quickLaunchModel.Url.ToUpper()));
            });
        }
    }
}
