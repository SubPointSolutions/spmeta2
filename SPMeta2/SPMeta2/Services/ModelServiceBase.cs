using System;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Events;
using SPMeta2.Utils;
using System.Reflection;

namespace SPMeta2.Services
{
    /// <summary>
    /// Base model service class for model provision operations.
    /// </summary>
    public abstract class ModelServiceBase : SPMetaServiceBase
    {
        #region constructors

        public ModelServiceBase()
        {
            TraceService = ServiceContainer.Instance.GetService<TraceServiceBase>();

            using (new TraceActivityScope(TraceService, (int)LogEventId.Initialization,
                string.Format("Initializing new ModelServiceBase instance of type: [{0}]", GetType())))
            {
                ModelHandlers = new Dictionary<Type, ModelHandlerBase>();
                ModelTraverseService = ServiceContainer.Instance.GetService<ModelTreeTraverseServiceBase>();

                // default pre-post deployment services
                PreDeploymentServices = new List<PreDeploymentServiceBase>();
                PreDeploymentServices.AddRange(ServiceContainer.Instance.GetServices<PreDeploymentServiceBase>());

                PostDeploymentServices = new List<PostDeploymentServiceBase>();
                PostDeploymentServices.AddRange(ServiceContainer.Instance.GetServices<PostDeploymentServiceBase>());
            }
        }

        #endregion

        #region static

        public static void RegisterModelHandlers<T>(ModelServiceBase instance)
            where T : ModelHandlerBase
        {
            RegisterModelHandlers<T>(instance, Assembly.GetExecutingAssembly());
        }

        public static void RegisterModelHandlers<T>(ModelServiceBase instance, Assembly asm)
            where T : ModelHandlerBase
        {
            var traceService = ServiceContainer.Instance.GetService<TraceServiceBase>();
            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<T>(asm);

            traceService.VerboseFormat((int)LogEventId.CoreCalls,
                  "RegisterModelHandlers for service:[{0}] and assembly: [{1}]",
                  new object[]
                    {
                        instance.GetType(),
                        asm.FullName
                    });

            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as T;

                if (handlerInstance != null)
                {
                    if (!instance.ModelHandlers.ContainsKey(handlerInstance.TargetType))
                    {
                        traceService.VerboseFormat((int)LogEventId.CoreCalls,
                           "Model handler for type [{0}] has not been registered yet. Registering.",
                           new object[] { handlerInstance.TargetType });

                        instance.ModelHandlers.Add(handlerInstance.TargetType, handlerInstance);
                    }
                    else
                    {
                        traceService.VerboseFormat((int)LogEventId.CoreCalls,
                            "Model handler for type [{0}] has been registered. Skipping.",
                           new object[] { handlerInstance.TargetType });
                    }
                }
            }
        }

        #endregion

        #region properties

        // public ModelWeighServiceBase ModelWeighService { get; set; }

        private readonly List<ModelHandlerBase> ModelHandlerEvents = new List<ModelHandlerBase>();
        private ModelNode _activeModelNode = null;

        public ModelTreeTraverseServiceBase ModelTraverseService { get; set; }

        public List<PreDeploymentServiceBase> PreDeploymentServices { get; set; }
        public List<PostDeploymentServiceBase> PostDeploymentServices { get; set; }

        #endregion

        #region events

        public EventHandler<ModelEventArgs> OnModelEvent;

        public EventHandler<OnModelNodeProcessedEventArgs> OnModelNodeProcessed;
        public EventHandler<OnModelNodeProcessingEventArgs> OnModelNodeProcessing;

        #endregion

        #region methods

        public void RegisterModelHandlers(Assembly assembly)
        {
            RegisterModelHandlers<ModelHandlerBase>(assembly);
        }

        public void RegisterModelHandlers<TModelHandlerBase>(Assembly assembly)
            where TModelHandlerBase : ModelHandlerBase
        {
            using (new TraceActivityScope(TraceService, (int)LogEventId.CoreCalls, string.Format("RegisterModelHandlers for assembly:[{0}]", assembly.FullName)))
            {
                RegisterModelHandlers<TModelHandlerBase>(this, assembly);
            }
        }

        public void RegisterModelHandler(ModelHandlerBase modelHandlerType)
        {
            TraceService.VerboseFormat((int)LogEventId.CoreCalls,
                  "RegisterModelHandler of type:[{0}] for target type:[{1}]",
                  new object[]
                    {
                       modelHandlerType.GetType(),
                       modelHandlerType.TargetType
                    });

            if (!ModelHandlers.ContainsKey(modelHandlerType.TargetType))
            {
                TraceService.VerboseFormat((int)LogEventId.CoreCalls,
                    "Model handler for type [{0}] has not been registered yet. Registering.",
                    new object[] { modelHandlerType.GetType() });

                ModelHandlers.Add(modelHandlerType.TargetType, modelHandlerType);
            }
            else
            {
                TraceService.VerboseFormat((int)LogEventId.CoreCalls,
                    "Model handler for type [{0}] has been registered. Skipping.",
                    new object[] { modelHandlerType.GetType() });
            }
        }

        public Dictionary<Type, ModelHandlerBase> ModelHandlers { get; set; }

        #endregion

        #region public API

        public virtual void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            RunPreDeploymentServices(modelHost, model);

            EnsureModelHandleEvents();
            ProcessModelDeployment(modelHost, model);

            RunPostDeploymentServices(modelHost, model);
        }

        private void RunPostDeploymentServices(ModelHostBase modelHost, ModelNode model)
        {
            var services = PostDeploymentServices.OrderBy(s => s.Order);

            foreach (var service in services)
                service.DeployModel(modelHost, model);
        }

        private void RunPreDeploymentServices(ModelHostBase modelHost, ModelNode model)
        {
            var services = PreDeploymentServices.OrderBy(s => s.Order);

            foreach (var service in services)
                service.DeployModel(modelHost, model);
        }

        public virtual void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            EnsureModelHandleEvents();

            // TMP, just web model is supported yet
            // experimental support yet
            // https://github.com/SubPointSolutions/spmeta2/issues/70

            var modelDefinition = model.Value;

            if (modelDefinition is WebDefinition)
                RetractWeb(modelHost, model);

            if (modelDefinition is SiteDefinition)
                RetractSite(modelHost, model);
        }

        #endregion

        #region protected

        protected void InvokeOnModelNodeProcessed(object sender, OnModelNodeProcessedEventArgs args)
        {
            if (OnModelNodeProcessed != null)
                OnModelNodeProcessed(sender, args);
        }

        protected void InvokeOnModelNodeProcessing(object sender, OnModelNodeProcessingEventArgs args)
        {
            if (OnModelNodeProcessing != null)
                OnModelNodeProcessing(sender, args);
        }

        protected virtual ModelHandlerBase ResolveModelHandler(Type modelType)
        {
            ModelHandlerBase result;

            ModelHandlers.TryGetValue(modelType, out result);

            return result;
        }

        private void EnsureModelHandleEvents()
        {
            using (new TraceMethodActivityScope(TraceService, (int)LogEventId.CoreCalls))
            {
                foreach (var modelHandler in ModelHandlers.Values
                    .Where(modelHandler => !ModelHandlerEvents.Contains(modelHandler)))
                {
                    modelHandler.OnModelEvent += (s, e) =>
                    {
                        if (e.CurrentModelNode != null)
                        {
                            e.CurrentModelNode.InvokeOnModelEvents(e.Object, e.EventType);
                            e.CurrentModelNode.InvokeOnModelContextEvents(s, e);
                        }
                        else if (_activeModelNode != null)
                        {
                            _activeModelNode.InvokeOnModelEvents(e.Object, e.EventType);
                            _activeModelNode.InvokeOnModelContextEvents(s, e);
                        }
                    };

                    ModelHandlerEvents.Add(modelHandler);
                }
            }
        }

        protected ModelHandlerBase ResolveModelHandlerForNode(ModelNode modelNode)
        {
            var modelDefinition = modelNode.Value as DefinitionBase;
            var modelHandler = ResolveModelHandler(modelDefinition.GetType());

            if (modelHandler == null)
            {
                TraceService.Error((int)LogEventId.ModelProcessingNullModelHandler, string.Format("Model handler instance is NULL. Throwing ArgumentNullException exception."));

                throw new ArgumentNullException(
                    string.Format("Can't find model handler for type:[{0}]. Current ModelService type: [{1}].",
                        modelDefinition.GetType(),
                        GetType()));
            }

            return modelHandler;
        }

        private void ProcessModelDeployment(object modelHost, ModelNode modelNode)
        {
            using (new TraceActivityScope(TraceService, (int)LogEventId.ModelProcessing, string.Format("ProcessModelDeployment for model:[{0}]", modelNode.Value.GetType())))
            {
                ModelTraverseService.OnModelHandlerResolve += ResolveModelHandlerForNode;

                // these are hack due event propagation mis-disign
                ModelTraverseService.OnModelProcessing += (node) =>
                {
                    _activeModelNode = node;
                };

                ModelTraverseService.OnModelProcessed += (node) =>
                {
                    _activeModelNode = null;
                };

                // on model fully-partially processed events
                ModelTraverseService.OnModelFullyProcessing += (node) =>
                {

                };

                ModelTraverseService.OnModelFullyProcessed += (node) =>
                {

                };

                ModelTraverseService.OnChildModelsProcessing += (node, type, childNodels) =>
                {

                };

                ModelTraverseService.OnChildModelsProcessed += (node, type, childNodels) =>
                {

                };

                ModelTraverseService.Traverse(modelHost, modelNode);
            }
        }

        private void RetractSite(object modelHost, ModelNode model)
        {
            // var siteDefinition = model.Value as SiteDefinition;
            // var modelHandler = ResolveModelHandler(siteDefinition.GetType());
        }

        private void RetractWeb(object modelHost, ModelNode model)
        {
            //var webDefinition = model.Value as WebDefinition;
            //var modelHandler = ResolveModelHandler(webDefinition.GetType());

            //if (modelHandler == null && webDefinition.RequireSelfProcessing)
            //    throw new ArgumentNullException(string.Format("Can't find model handler for type:[{0}] ", webDefinition.GetType()));

            //modelHandler.RetractModel(modelHost, webDefinition);
        }

        #endregion
    }


}
