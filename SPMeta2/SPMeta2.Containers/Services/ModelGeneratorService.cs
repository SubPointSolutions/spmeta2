using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Utils;

namespace SPMeta2.Containers.Services
{
    public class ModelGeneratorService
    {
        public ModelGeneratorService()
        {
            DefinitionGenerators = new Dictionary<Type, DefinitionGeneratorServiceBase>();

            RegisterDefinitionGenerators(Assembly.GetExecutingAssembly());
            RegisterDefinitionGenerators(typeof(ListViewDefinitionGenerator).Assembly);
        }

        public Dictionary<Type, DefinitionGeneratorServiceBase> DefinitionGenerators { get; set; }

        public void RegisterDefinitionGenerators(Assembly targetAssembly)
        {
            var handlerTypes = ReflectionUtils
                .GetTypesFromAssembly<DefinitionGeneratorServiceBase>(targetAssembly);


            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as DefinitionGeneratorServiceBase;

                if (handlerInstance != null)
                {
                    if (!DefinitionGenerators.ContainsKey(handlerInstance.TargetType))
                        DefinitionGenerators.Add(handlerInstance.TargetType, handlerInstance);
                }
            }
        }

        private class GeneratedArtifact
        {
            public DefinitionBase Definition { get; set; }
            public IEnumerable<string> AdditionalDefinitions { get; set; }

        }

        public IEnumerable<DefinitionBase> GetAdditionalDefinition<TDefinition>()
        {
            return GetAdditionalDefinitions(typeof(TDefinition));
        }

        public DefinitionBase CurrentDefinition { get; set; }

        public ModelNode GenerateModelTreeForDefinition<TDefinition>(SPObjectModelType objectModelType)
             where TDefinition : DefinitionBase, new()
        {
            ModelNode result = null;

            var rootHostType = GetRootHostType<TDefinition>(objectModelType);
            var parentHostType = GetParentHostType<TDefinition>(objectModelType);

            var currentDefinition = GetRandomDefinition<TDefinition>();

            CurrentDefinition = currentDefinition;

            var defs = new List<GeneratedArtifact>();

            LookupModelTree<TDefinition>(rootHostType, defs, objectModelType);

            if (defs.Count > 0)
            {
                defs.Reverse();

                var rootHost = defs[0].Definition;
                parentHostType = rootHost.GetType();

                defs.RemoveAt(0);

                ModelNode model = null;

                if (parentHostType == typeof(FarmDefinition))
                    model = SPMeta2Model.NewFarmModel();

                if (parentHostType == typeof(WebApplicationDefinition))
                    model = SPMeta2Model.NewWebApplicationModel();

                if (parentHostType == typeof(SiteDefinition))
                    model = SPMeta2Model.NewSiteModel();

                if (parentHostType == typeof(WebDefinition))
                    model = SPMeta2Model.NewWebModel();

                var _m = model;

                foreach (var def in defs)
                {
                    _m
                       .AddDefinitionNode(def.Definition, currentDef =>
                       {
                           currentDef.Options.RequireSelfProcessing = def.Definition.RequireSelfProcessing;

                           _m = currentDef;
                       });
                }

                _m.AddDefinitionNode(currentDefinition);


                result = model;
            }
            else
            {
                ModelNode resultModel = null;

                if (currentDefinition.GetType() == typeof(FarmDefinition))
                    resultModel = SPMeta2Model.NewFarmModel();

                if (currentDefinition.GetType() == typeof(WebApplicationDefinition))
                    resultModel = SPMeta2Model.NewWebApplicationModel();

                if (currentDefinition.GetType() == typeof(SiteDefinition))
                    resultModel = SPMeta2Model.NewSiteModel();

                if (currentDefinition.GetType() == typeof(WebDefinition))
                    resultModel = SPMeta2Model.NewWebModel();

                if (resultModel == null)
                {
                    throw new SPMeta2NotImplementedException(string.Format("Cannot find host model for type:[{0}]. Ensure correct RootHostAttribute/ParentHostAttribute/SPObjectTypeAttribute attributes.", typeof(TDefinition).AssemblyQualifiedName));
                }

                resultModel.Value = currentDefinition;
                result = resultModel;
            }

            // ProcessAdditionalArtifacts(result, defs);

            return result;
        }

        private void ProcessAdditionalArtifacts(ModelNode model, List<GeneratedArtifact> defs)
        {
            foreach (var gen in defs)
            {
                if (gen.AdditionalDefinitions == null)
                    continue;

                foreach (var def in gen.AdditionalDefinitions)
                {

                }
            }
        }

        private void LookupModelTree<TDefinition>(Type rootHostType, List<GeneratedArtifact> defs, SPObjectModelType objectModelType)
        {
            LookupModelTree(rootHostType, typeof(TDefinition), defs, objectModelType);
        }

        public void ComposeModelWithAdditionalDefinitions(ModelNode definitionSandbox,
            IEnumerable<DefinitionBase> additionalDefinitions,
             SPObjectModelType objectModelType)
        {
            if (additionalDefinitions == null)
                return;

            foreach (var definition in additionalDefinitions)
            {
                var parentHostType = GetParentHostType(definition.GetType(), objectModelType);

                var targetNode = definitionSandbox
                                        .FindNodes(model =>
                                        {
                                            var featureDefinition = definition as FeatureDefinition;

                                            if (featureDefinition != null)
                                            {
                                                if (featureDefinition.Scope == FeatureDefinitionScope.Web)
                                                    return model.Value.GetType() == typeof(WebDefinition) || model.Value.GetType() == typeof(RootWebDefinition);

                                                if (featureDefinition.Scope == FeatureDefinitionScope.Site)
                                                    return model.Value.GetType() == typeof(SiteDefinition);
                                            }

                                            return model.Value.GetType() == parentHostType;
                                        })
                                        .FirstOrDefault();

                // additional definitions should be 'first' 
                // no NULL ref check, as it means that the model was constructed in the wrong way
                targetNode
                    .ChildModels.Insert(0, new ModelNode { Value = definition });

            }
        }

        private ModelNode LookupNodelNodeForDefinition(ModelNode node)
        {
            return null;
        }

        private void LookupModelTree(Type rootHostType, Type type, List<GeneratedArtifact> defs, SPObjectModelType objectModelType)
        {
            if (rootHostType == null)
                return;

            if (rootHostType == type)
                return;

            var parentHostType = type;
            var customParentHost = GetDefinitionParentHost(type);

            if (customParentHost == null)
            {
                var upParentHostType = GetParentHostType(parentHostType, objectModelType);

                var definition = GetRandomDefinition(upParentHostType);
                defs.Add(new GeneratedArtifact
                {
                    Definition = definition
                });

                var additionalParentHostTypes = GetParentHostAdditionalTypes(type, objectModelType);

                Type lastHostType = null;

                foreach (var additionalParentHostType in additionalParentHostTypes)
                {
                    var def = GetRandomDefinition(additionalParentHostType);
                    defs.Add(new GeneratedArtifact
                    {
                        Definition = def
                    });

                    lastHostType = additionalParentHostType;
                }

                if (lastHostType != null)
                    upParentHostType = GetParentHostType(lastHostType, objectModelType);

                LookupModelTree(rootHostType, upParentHostType, defs, objectModelType);
            }
            else
            {
                defs.Add(new GeneratedArtifact
                {
                    Definition = customParentHost
                });

                var additionalParentHostTypes = GetParentHostAdditionalTypes(type, objectModelType);

                Type lastHostType = null;

                foreach (var additionalParentHostType in additionalParentHostTypes)
                {
                    var def = GetRandomDefinition(additionalParentHostType);
                    defs.Add(new GeneratedArtifact
                    {
                        Definition = def
                    });

                    lastHostType = additionalParentHostType;
                }

                if (lastHostType != null)
                    LookupModelTree(rootHostType, lastHostType, defs, objectModelType);
                else
                    LookupModelTree(rootHostType, customParentHost.GetType(), defs, objectModelType);
            }
        }



        private DefinitionBase GetDefinitionParentHost(Type type)
        {
            if (!DefinitionGenerators.ContainsKey(type))
                throw new SPMeta2NotImplementedException(string.Format("Cannot find definition generator for type:[{0}]", type.AssemblyQualifiedName));

            var definitionGenrator = DefinitionGenerators[type];

            return definitionGenrator.GetCustomParenHost();
        }

        public TDefinition GetRandomDefinition<TDefinition>()
            where TDefinition : DefinitionBase
        {
            return GetRandomDefinition<TDefinition>(null);
        }

        public TDefinition GetRandomDefinition<TDefinition>(Action<TDefinition> action)
           where TDefinition : DefinitionBase
        {
            var result = (TDefinition)GetRandomDefinition(typeof(TDefinition));

            if (action != null)
                action(result);

            return result;
        }

        public DefinitionBase GetRandomDefinition(Type type)
        {
            if (!DefinitionGenerators.ContainsKey(type))
                throw new SPMeta2NotImplementedException(string.Format("Cannot find definition generator for type:[{0}]", type.AssemblyQualifiedName));

            var definitionGenrator = DefinitionGenerators[type];
            var definition = definitionGenrator.GenerateRandomDefinition();

            // this is a pure gold
            // we need to check if generator creates exactly what it is supposed to create
            // slips are about having inheritance such as Field -> ChoiceField -> MultiChoiceField
            if (definition.GetType() == definitionGenrator.TargetType)
            {
                return definition;
            }

            throw new SPMeta2Exception(
                string.Format("Definition generator type mismatch. Expected: [{0}] Actual: [{1}]",
                new[]
                {
                    definitionGenrator.TargetType,
                    definition.GetType()
                }));
        }

        private IEnumerable<DefinitionBase> GetAdditionalDefinitions(Type type)
        {
            if (!DefinitionGenerators.ContainsKey(type))
                throw new SPMeta2NotImplementedException(string.Format("Cannot find definition generator for type:[{0}]", type.AssemblyQualifiedName));

            var definitionGenrator = DefinitionGenerators[type];

            return definitionGenrator.GetAdditionalArtifacts();
        }


        private Type GetRootHostType<TDefinition>(SPObjectModelType objectModelType)
        {
            var hostAtrrs = typeof(TDefinition)
                                       .GetCustomAttributes(false)
                                       .OfType<DefaultRootHostAttribute>()
                                       .ToList();

            if (hostAtrrs.Count == 0)
                return null;

            if (hostAtrrs.Count == 1)
                return hostAtrrs[0].HostType;

            switch (objectModelType)
            {
                case SPObjectModelType.CSOM:
                    {
                        var csomHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(CSOMRootHostAttribute));
                        return csomHost.HostType;

                    }; break;

                case SPObjectModelType.SSOM:
                    {
                        var defaultHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(DefaultRootHostAttribute));
                        return defaultHost.HostType;

                    }; break;
            }


            throw new SPMeta2NotImplementedException(string.Format("Unsupported SPObjectModelType:[{0}]", objectModelType));

        }

        private Type GetParentHostType<TDefinition>(SPObjectModelType objectModelType)
        {
            return GetParentHostType(typeof(TDefinition), objectModelType);
        }

        private Type GetParentHostType(Type type, SPObjectModelType objectModelType)
        {
            var hostAtrrs = type
                                      .GetCustomAttributes(false)
                                      .OfType<DefaultParentHostAttribute>()
                                      .ToList();

            if (hostAtrrs.Count == 0)
                return null;

            if (hostAtrrs.Count == 1)
                return hostAtrrs[0].HostType;

            switch (objectModelType)
            {
                case SPObjectModelType.CSOM:
                    {
                        var csomHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(CSOMParentHostAttribute));
                        return csomHost.HostType;

                    }; break;

                case SPObjectModelType.SSOM:
                    {
                        var defaultHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(DefaultParentHostAttribute));
                        return defaultHost.HostType;

                    }; break;
            }


            throw new SPMeta2NotImplementedException(string.Format("Unsupported SPObjectModelType:[{0}]", objectModelType));

        }

        private IEnumerable<Type> GetParentHostAdditionalTypes(Type type, SPObjectModelType objectModelType)
        {
            var hostAtrrs = type
                                      .GetCustomAttributes(false)
                                      .OfType<DefaultParentHostAttribute>()
                                      .ToList();

            if (hostAtrrs.Count == 0)
                return null;

            if (hostAtrrs.Count == 1)
                return hostAtrrs[0].AdditionalHostTypes;

            switch (objectModelType)
            {
                case SPObjectModelType.CSOM:
                    {
                        var csomHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(CSOMParentHostAttribute));
                        return csomHost.AdditionalHostTypes;

                    }; break;

                case SPObjectModelType.SSOM:
                    {
                        var defaultHost = hostAtrrs.FirstOrDefault(a => a.GetType() == typeof(DefaultParentHostAttribute));
                        return defaultHost.AdditionalHostTypes;

                    }; break;
            }


            throw new SPMeta2NotImplementedException(string.Format("Unsupported SPObjectModelType:[{0}]", objectModelType));

        }


    }
}
