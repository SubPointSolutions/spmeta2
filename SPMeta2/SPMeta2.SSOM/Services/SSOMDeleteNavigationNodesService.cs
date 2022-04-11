using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Navigation;
using SPMeta2.Definitions.Base;

namespace SPMeta2.SSOM.Services
{
    public class SSOMDeleteNavigationNodesService
    {

        public virtual void DeleteNodesByMatch(DeleteNavigationNodesDefinitionBase typedDefinition,
            SPNavigationNodeCollection nodesCollection,
            Func<string, string> resolveTokenizedUrlAction)
        {
            var allNodes = nodesCollection.OfType<SPNavigationNode>()
                                             .ToArray();

            var nodesToDelete = FindMatches(allNodes, typedDefinition, resolveTokenizedUrlAction);

            if (nodesToDelete.Any())
            {
                foreach (var node in nodesToDelete.ToArray())
                {
                    node.Delete();
                }
            }
        }

        public virtual List<SPNavigationNode> FindMatches(
            SPNavigationNode[] allNodes,
            DeleteNavigationNodesDefinitionBase typedDefinition,
            Func<string, string> resolveTokenizedUrlAction)
        {
            var nodesToDelete = new List<SPNavigationNode>();

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
