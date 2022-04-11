using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.Exceptions;

using Microsoft.SharePoint;
using System.IO;
using System.Xml;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Regression.CSOM;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class DeleteTopNavigationNodesDefinitionValidator : DeleteTopNavigationNodesModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DeleteTopNavigationNodesDefinition>("model", value => value.RequireNotNull());

            var spObject = host.HostWeb;
            var context = spObject.Context;

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.NavigationNodes);

                // we should not find any matched
                // they should have been already deleted by the model handler
                var nodesCollection = GetNavigationNodeCollection(spObject);

                context.Load(nodesCollection);
                context.ExecuteQueryWithTrace();

                var matches = DeleteNavigationNodesService.FindMatches(
                        nodesCollection.ToArray(),
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
