using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.CSOM.Services
{
    public class CSOMDeleteNavigationNodesService
    {

        public virtual void DeleteNodesByMatch(DeleteNavigationNodesDefinitionBase typedDefinition,
            NavigationNodeCollection nodesCollection,
            Func<string, string> resolveTokenizedUrlAction)
        {
            var allNodes = nodesCollection.ToArray();
            var nodesToDelete = FindMatches(allNodes, typedDefinition, resolveTokenizedUrlAction);

            if (nodesToDelete.Any())
            {
                var context = nodesCollection.Context;

                foreach (var node in nodesToDelete.ToArray())
                {
                    node.DeleteObject();
                }

                context.ExecuteQueryWithTrace();
            }
        }

        public virtual List<NavigationNode> FindMatches(
            NavigationNode[] allNodes,
            DeleteNavigationNodesDefinitionBase typedDefinition,
            Func<string, string> resolveTokenizedUrlAction)
        {
            var nodesToDelete = new List<NavigationNode>();

            foreach (var nodeMatch in typedDefinition.NavigationNodes)
            {
                var foundByTitle = false;

                // search by Title, first
                if (!string.IsNullOrEmpty(nodeMatch.Title))
                {
                    var nodeByTitle = allNodes.FirstOrDefault(f =>
                        string.Equals(f.Title, nodeMatch.Title, StringComparison.OrdinalIgnoreCase));

                    if (nodeByTitle != null)
                    {
                        foundByTitle = true;

                        if (!nodesToDelete.Contains(nodeByTitle))
                        {
                            nodesToDelete.Add(nodeByTitle);
                        }
                    }
                }

                // give a try by Url, then
                if (!foundByTitle && !string.IsNullOrEmpty(nodeMatch.Url))
                {
                    var matchUrl = resolveTokenizedUrlAction(nodeMatch.Url);

                    // special char resolution, manual fix to avoid 
                    // Add more tests for DeleteXXXnavigationNode scenarios #864
                    // https://github.com/SubPointSolutions/spmeta2/issues/864
                    matchUrl = HttpUtility.HtmlEncode(matchUrl);

                    var nodeByUrl = allNodes.FirstOrDefault(f =>
                        !string.IsNullOrEmpty(f.Url)
                        && f.Url.EndsWith(matchUrl, StringComparison.OrdinalIgnoreCase));

                    if (nodeByUrl != null)
                    {
                        if (!nodesToDelete.Contains(nodeByUrl))
                            nodesToDelete.Add(nodeByUrl);
                    }
                }
            }
            return nodesToDelete;
        }
    }
}
