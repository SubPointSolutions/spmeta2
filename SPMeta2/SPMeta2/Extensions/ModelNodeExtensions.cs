using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Extensions
{

    public static class ModelNodeExtensions
    {
        #region static

        public static ModelNode WithNodes<T>(this ModelNode model, T definition)
            where T : DefinitionBase
        {
            return WithNode(model, definition);

        }

        public static ModelNode WithNodes<T>(this ModelNode model, T definition, Action<ModelNode> action)
           where T : DefinitionBase
        {
            var nodes = FindNodes(model, definition);

            if (action != null)
            {
                foreach (var node in nodes)
                {
                    action(node);
                }
            }

            return model;
        }

        public static ModelNode WithNode<T>(this ModelNode model, T definition)
            where T : DefinitionBase
        {
            return WithNode(model, definition);

        }

        public static ModelNode WithNode<T>(this ModelNode model, T definition, Action<ModelNode> action)
            where T : DefinitionBase
        {
            var node = FindFirstOrDefaultNode(model, definition);

            if (node != null && action != null)
            {
                action(node);
            }

            return model;
        }

        public static ModelNode FindFirstOrDefaultNode(this ModelNode model, DefinitionBase definition)
        {
            return FindNodes(model, definition).FirstOrDefault();
        }

        public static List<ModelNode> FindNodes(this ModelNode model, DefinitionBase definition)
        {
            return FindNodes(model, modelNode =>
              {
                  return modelNode.Value == definition;
              });
        }

        public static List<ModelNode> FindNodes(this ModelNode model, Func<ModelNode, bool> match)
        {
            var result = new List<ModelNode>();

            if (match(model))
                result.Add(model);

            foreach (var node in model.ChildModels)
            {
                var tmpNodes = FindNodes(node, match);

                foreach (var tmpNode in tmpNodes)
                {
                    if (!result.Contains(tmpNode))
                        result.Add(tmpNode);
                }
            }

            return result;
        }

        public static ModelNode WithNodesOfType<TModelDefinition>(this ModelNode model, Action<ModelNode> action)
           where TModelDefinition : DefinitionBase
        {
            if (action == null)
                return model;

            var nodes = FindNodes(model, modelNode =>
            {
                return modelNode.Value is TModelDefinition;
            });

            for (int i = 0; i < nodes.Count; i++)
            {
                action(nodes[i]);
            }

            return model;
        }

        #endregion
    }
}
