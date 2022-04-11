using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Common;
using SPMeta2.Utils;

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
            return FindNodes(model, modelNode => modelNode.Value == definition);
        }

        public static List<ModelNode> Flatten(this ModelNode model)
        {
            return Flatten(model, node => true);
        }

        public static List<ModelNode> Flatten(this ModelNode model, Func<ModelNode, bool> match)
        {
            var result = new List<ModelNode>();

            //if (match != null)
            //{
            //    if (match(model))
            //        result.Add(model);
            //}
            //else
            //{
            //    result.Add(model);
            //}

            result.AddRange(FindNodes(model, match));

            return result;
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

            var nodes = FindNodes(model, modelNode => modelNode.Value is TModelDefinition);

            foreach (var n in nodes)
                action(n);

            return model;
        }

        #endregion

        #region import

        /// <summary>
        /// Imports all definitions from the static class fields / props.
        /// </summary>
        /// <param name="node"></param>
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

        #region print extensions

        /// <summary>
        /// Renders readable, tree view looking-like model view as a string
        /// More details - https://github.com/SubPointSolutions/spmeta2/issues/826
        /// </summary>
        /// <param name="modelNode"></param>
        /// <returns></returns>
        public static string ToPrettyPrint(this ModelNode modelNode)
        {
            var service = ServiceContainer.Instance.GetService<ModelPrettyPrintServiceBase>();

            return service.PrintModel(modelNode);
        }

        /// <summary>
        /// Generates graph in DOT notation
        /// Use http://www.webgraphviz.com to visualize it fast
        /// Mere details - https://github.com/SubPointSolutions/spmeta2/issues/845
        /// </summary>
        /// <param name="modelNode"></param>
        /// <returns></returns>
        public static string ToDotGraph(this ModelNode modelNode)
        {
            var service = ServiceContainer.Instance.GetService<ModelDotGraphPrintServiceBase>();

            return service.PrintModel(modelNode);
        }

        #endregion

        #region validation

        public static ModelValidationResultSet<DefinitionBase> Validate(this ModelNode node, Action<TypedModelValidationResult<DefinitionBase>> action)
        {
            var service = ServiceContainer.Instance.GetService<FluentModelValidationServiceBase>();

            return service.Validate<DefinitionBase>(node, action);
        }

        public static ModelValidationResultSet<TDefinition> Validate<TDefinition>(this ModelNode node, Action<TypedModelValidationResult<TDefinition>> action)
            where TDefinition : DefinitionBase
        {
            var service = ServiceContainer.Instance.GetService<FluentModelValidationServiceBase>();

            return service.Validate(node, action);
        }

        #endregion

        #region model node properties

        internal static void InternalSetPropertyBagValue(List<PropertyBagValue> values,
            string name,
            string value)
        {
            var currentValue = values.FirstOrDefault(p => !string.IsNullOrEmpty(p.Name)
                                                            && p.Name.ToUpper() == name.ToUpper());

            if (currentValue == null)
            {
                currentValue = new PropertyBagValue
                {
                    Name = name,
                    Value = value
                };

                values.Add(currentValue);
            }

            currentValue.Value = value;
        }

        internal static string InternalGetPropertyBagValue(List<PropertyBagValue> values,
            string name)
        {
            var currentValue = values.FirstOrDefault(p => !string.IsNullOrEmpty(p.Name)
                                                            && p.Name.ToUpper() == name.ToUpper());

            if (currentValue != null)
            {
                return currentValue.Value;
            }

            return null;
        }

        public static TModelNode SetNonPersistentPropertyBagValue<TModelNode>(this TModelNode modelNode,
            string name,
            string value)
            where TModelNode : ModelNode
        {
            InternalSetPropertyBagValue(modelNode.NonPersistentPropertyBag, name, value);

            return modelNode;
        }


        public static string GetNonPersistentPropertyBagValue<TModelNode>(this TModelNode modelNode,
              string name)
              where TModelNode : ModelNode
        {
            return InternalGetPropertyBagValue(modelNode.NonPersistentPropertyBag, name);
        }
        public static TModelNode SetPropertyBagValue<TModelNode>(this TModelNode modelNode,
            string name,
            string value)
            where TModelNode : ModelNode
        {
            InternalSetPropertyBagValue(modelNode.PropertyBag, name, value);

            return modelNode;
        }

        public static string GetPropertyBagValue<TModelNode>(this TModelNode modelNode,
            string name)
            where TModelNode : ModelNode
        {
            return InternalGetPropertyBagValue(modelNode.PropertyBag, name);
        }

        #endregion

        #region incremental provision

        public static bool GetIncrementalRequireSelfProcessingValue<TModelNode>(this TModelNode modelNode)
            where TModelNode : ModelNode
        {
            var incrementalRequireSelfProcessingValue = modelNode.GetNonPersistentPropertyBagValue(DefaultModelNodePropertyBagValue.Sys.IncrementalRequireSelfProcessingValue);

            if (incrementalRequireSelfProcessingValue != null)
                return ConvertUtils.ToBoolWithDefault(incrementalRequireSelfProcessingValue, false);

            return false;
        }

        internal static TModelNode InternalSetIncrementalProvisionModelId<TModelNode>(this TModelNode modelNode, string modelId)
            where TModelNode : ModelNode
        {
            modelNode.SetPropertyBagValue(DefaultModelNodePropertyBagValue.Sys.IncrementalProvision.PersistenceStorageModelId, modelId);

            return modelNode;
        }

        public static FarmModelNode SetIncrementalProvisionModelId(this FarmModelNode modelNode, string modelId)
        {
            return InternalSetIncrementalProvisionModelId(modelNode, modelId);
        }

        public static WebApplicationModelNode SetIncrementalProvisionModelId(this WebApplicationModelNode modelNode, string modelId)
        {
            return InternalSetIncrementalProvisionModelId(modelNode, modelId);
        }

        public static SiteModelNode SetIncrementalProvisionModelId(this SiteModelNode modelNode, string modelId)
        {
            return InternalSetIncrementalProvisionModelId(modelNode, modelId);
        }

        public static WebModelNode SetIncrementalProvisionModelId(this WebModelNode modelNode, string modelId)
        {
            return InternalSetIncrementalProvisionModelId(modelNode, modelId);
        }

        #endregion

        #region provision compatibility

        public static ModelProvisionCompatibilityResult CheckProvisionCompatibility(this ModelNode modelNode)
        {
            var service = ServiceContainer.Instance.GetService<ModelCompatibilityServiceBase>();

            return service.CheckProvisionCompatibility(modelNode);
        }

        public static bool IsCSOMCompatible(this ModelNode modelNode)
        {
            var compatibilityResult = CheckProvisionCompatibility(modelNode);
            var result = compatibilityResult.Result.All(r => r.IsCSOMCompatible.HasValue
                                                       && r.IsCSOMCompatible.Value);

            return result;
        }

        public static bool IsSSOMCompatible(this ModelNode model)
        {
            var compatibilityResult = CheckProvisionCompatibility(model);
            var result = compatibilityResult.Result.All(r => r.IsSSOMCompatible.HasValue
                                                             && r.IsSSOMCompatible.Value);

            return result;
        }

        #endregion
    }
}
