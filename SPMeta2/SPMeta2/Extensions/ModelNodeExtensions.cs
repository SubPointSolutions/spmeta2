using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        #region import

        /// <summary>
        /// Imports all definitions from the static class fields / props.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="classType"></param>
        public static ModelNode AddDefinitionsFromStaticClassType<TType>(this ModelNode node)
        {
            return AddDefinitionsFromStaticClassType(node, typeof(TType));
        }

        /// <summary>
        /// Imports all definitions from the static class fields.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="classType"></param>
        public static ModelNode AddDefinitionsFromStaticClassType(this ModelNode node, Type classType)
        {
            foreach (var field in classType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var definition = field.GetValue(null) as DefinitionBase;

                if (definition != null)
                    node.CreateDefinitionNode(definition);
            }

            foreach (var prop in classType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                var definition = prop.GetValue(null, null) as DefinitionBase;

                if (definition != null)
                    node.CreateDefinitionNode(definition);
            }

            return node;
        }

        internal static ModelNode CreateDefinitionNode(this ModelNode node, DefinitionBase definition)
        {
            return CreateDefinitionNode(node, definition, null);
        }

        internal static ModelNode CreateDefinitionNode(this ModelNode node, DefinitionBase definition,
             Action<ModelNode> action)
        {
            var modelNode = new ModelNode { Value = definition };

            node.ChildModels.Add(modelNode);

            if (action != null)
                action(modelNode);

            return node;
        }


        #endregion
    }
}
