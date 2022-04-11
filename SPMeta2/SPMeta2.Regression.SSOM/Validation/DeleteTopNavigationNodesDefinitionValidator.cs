using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.Extensions;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.Exceptions;

using Microsoft.SharePoint;
using System.IO;
using System.Xml;
using Microsoft.SharePoint.Navigation;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class DeleteTopNavigationNodesDefinitionValidator : DeleteTopNavigationNodesModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DeleteTopNavigationNodesDefinition>("model", value => value.RequireNotNull());

            var spObject = host.HostWeb;

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.NavigationNodes);

                // we should not find any matched
                // they should have been already deleted by the model handler
                var nodesCollection = GetNavigationNodeCollection(spObject);
                var matches = DeleteNavigationNodesService.FindMatches(
                        nodesCollection.OfType<SPNavigationNode>().ToArray(),
                        definition,
                        url =>
                        {
                            return ResolveTokenizedUrl(host, url);
                        });

                var isValid = matches.Count == 0;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }
    }


}
