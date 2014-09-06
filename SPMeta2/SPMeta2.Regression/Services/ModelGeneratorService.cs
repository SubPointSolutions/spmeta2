using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Attributes.Regression;

using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using SPMeta2.Utils;
using System.Reflection;
using SPMeta2.Exceptions;
using SPMeta2.Attributes;

namespace SPMeta2.Regression.Services
{
    public class ModelGeneratorService
    {
        public ModelGeneratorService()
        {
            RegisterDefinitionGenerators();
        }

        private Dictionary<Type, DefinitionGeneratorServiceBase> DefinitionGenerators = new Dictionary<Type, DefinitionGeneratorServiceBase>();

        private void RegisterDefinitionGenerators()
        {
            DefinitionGenerators.Clear();

            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<DefinitionGeneratorServiceBase>(Assembly.GetExecutingAssembly());

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

        public ModelNode GenerateModelTreeForDefinition<TDefinition>(SPObjectModelType objectModelType)
             where TDefinition : DefinitionBase, new()
        {
            var rootHostType = GetRootHostType<TDefinition>(objectModelType);
            var parentHostType = GetParentHostType<TDefinition>(objectModelType);

            var currentDefinition = GetRandomDefinition<TDefinition>();
            var defs = new List<DefinitionBase>();

            LookupModelTree<TDefinition>(rootHostType, defs, objectModelType);

            if (defs.Count > 0)
            {
                defs.Reverse();

                var rootHost = defs[0];
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
                   .AddDefinitionNode(def, currentDef =>
                   {
                       _m = currentDef;

                   });
                }

                _m.AddDefinitionNode(currentDefinition);


                return model;
            }

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

            return resultModel;
        }

        private void LookupModelTree<TDefinition>(Type rootHostType, List<DefinitionBase> defs, SPObjectModelType objectModelType)
        {
            LookupModelTree(rootHostType, typeof(TDefinition), defs, objectModelType);
        }

        private void LookupModelTree(Type rootHostType, Type type, List<DefinitionBase> defs, SPObjectModelType objectModelType)
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
                defs.Add(definition);

                LookupModelTree(rootHostType, upParentHostType, defs, objectModelType);
            }
            else
            {
                defs.Add(customParentHost);
                LookupModelTree(rootHostType, customParentHost.GetType(), defs, objectModelType);
            }
        }

        private TDefinition GetRandomDefinition<TDefinition>()
            where TDefinition : DefinitionBase
        {
            return (TDefinition)GetRandomDefinition(typeof(TDefinition));
        }

        private DefinitionBase GetDefinitionParentHost(Type type)
        {
            if (!DefinitionGenerators.ContainsKey(type))
                throw new SPMeta2NotImplementedException(string.Format("Cannot find definition generator for type:[{0}]", type.AssemblyQualifiedName));

            var definitionGenrator = DefinitionGenerators[type];

            return definitionGenrator.GetCustomParenHost();
        }

        private DefinitionBase GetRandomDefinition(Type type)
        {
            if (!DefinitionGenerators.ContainsKey(type))
                throw new SPMeta2NotImplementedException(string.Format("Cannot find definition generator for type:[{0}]", type.AssemblyQualifiedName));

            var definitionGenrator = DefinitionGenerators[type];

            return definitionGenrator.GenerateRandomDefinition();
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


                    }; break;
            }


            throw new SPMeta2NotImplementedException(string.Format("Unsupporter SPObjectModelType:[{0}]", objectModelType));

        }
    }
}
