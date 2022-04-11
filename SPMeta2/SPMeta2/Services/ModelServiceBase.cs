using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services.Impl;
using SPMeta2.Utils;

namespace SPMeta2.Services
{

    public class ModelProcessingEventArgs : EventArgs
    {
        public ModelNode Model { get; set; }
        public ModelNode CurrentNode { get; set; }

        public int ProcessedModelNodeCount { get; set; }

        public int TotalModelNodeCount
        {
            get
            {
                if (Model == null)
                    return 0;

                var result = 0;

                Model.WithNodesOfType<DefinitionBase>(n => { result++; });

                return result;
            }
        }
    }

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

                // should be a new instance all the time
                //// ServiceContainer.Instance.GetService<ModelTreeTraverseServiceBase>();
                //ModelTraverseService = new DefaultModelTreeTraverseService();



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

        public Type CustomModelTreeTraverseServiceType { get; set; }

        // public ModelWeighServiceBase ModelWeighService { get; set; }

        private readonly List<ModelHandlerBase> _modelHandlerEvents = new List<ModelHandlerBase>();
        private ModelNode _activeModelNode = null;

        private ModelTreeTraverseServiceBase _modelTraverseService;

        public ModelTreeTraverseServiceBase ModelTraverseService
        {
            get
            {
                if (_modelTraverseService == null)
                {
                    if (CustomModelTreeTraverseServiceType != null)
                    {
                        _modelTraverseService = ServiceContainer.Instance
                                                               .CreateNewInstance(CustomModelTreeTraverseServiceType) as ModelTreeTraverseServiceBase;
                    }
                    else
                    {
                        _modelTraverseService = ServiceContainer.Instance
                                                               .CreateNewService<ModelTreeTraverseServiceBase>();
                    }

                    InitModelTraverseService();
                }

                return _modelTraverseService;
            }
            set { _modelTraverseService = value; }
        }

        public List<PreDeploymentServiceBase> PreDeploymentServices { get; set; }
        public List<PostDeploymentServiceBase> PostDeploymentServices { get; set; }

        #endregion

        #region events

        public EventHandler<ModelProcessingEventArgs> OnModelNodeProcessed;
        public EventHandler<ModelProcessingEventArgs> OnModelNodeProcessing;

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
                    .Where(modelHandler => !_modelHandlerEvents.Contains(modelHandler)))
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

                    _modelHandlerEvents.Add(modelHandler);
                }
            }
        }

        public static Func<ModelNode, ModelHandlerBase> OnResolveNullModelHandler;

        protected virtual ModelHandlerBase ResolveModelHandlerForNode(ModelNode modelNode)
        {
            var modelDefinition = modelNode.Value as DefinitionBase;
            var modelHandler = ResolveModelHandler(modelDefinition.GetType());

            if (modelHandler == null)
            {
                if (OnResolveNullModelHandler != null)
                    return OnResolveNullModelHandler(modelNode);

                TraceService.Error((int)LogEventId.ModelProcessingNullModelHandler, string.Format("Model handler instance is NULL. Throwing ArgumentNullException exception."));

                throw new ArgumentNullException(
                    string.Format("Can't find model handler for type:[{0}]. Current ModelService type: [{1}].",
                        modelDefinition.GetType(),
                        GetType()));
            }

            return modelHandler;
        }


        protected virtual void RaiseOnModelNodeProcessing(object sender, ModelProcessingEventArgs args)
        {
            if (OnModelNodeProcessing != null)
                OnModelNodeProcessing(sender, args);
        }

        protected virtual void RaiseOnModelNodeProcessed(object sender, ModelProcessingEventArgs args)
        {
            if (OnModelNodeProcessed != null)
                OnModelNodeProcessed(sender, args);
        }
        private void InitModelTraverseService()
        {
            _modelTraverseService.OnModelHandlerResolve += ResolveModelHandlerForNode;

            // these are hack due event propagation mis-disign
            _modelTraverseService.OnModelProcessing += (node) =>
            {
                RaiseOnModelNodeProcessing(this, new ModelProcessingEventArgs
                {
                    Model = CurrentModelNode,
                    CurrentNode = node,
                    ProcessedModelNodeCount = CurrentModelNodeIndex
                });

                _activeModelNode = node;
            };

            _modelTraverseService.OnModelProcessed += (node) =>
            {
                RaiseOnModelNodeProcessed(this, new ModelProcessingEventArgs
                {
                    Model = CurrentModelNode,
                    CurrentNode = node,
                    ProcessedModelNodeCount = CurrentModelNodeIndex
                });

                CurrentModelNodeIndex++;

                _activeModelNode = null;
            };

            // on model fully-partially processed events
            _modelTraverseService.OnModelFullyProcessing += (node) =>
            {

            };

            _modelTraverseService.OnModelFullyProcessed += (node) =>
            {

            };

            _modelTraverseService.OnChildModelsProcessing += (node, type, childNodels) =>
            {

            };

            _modelTraverseService.OnChildModelsProcessed += (node, type, childNodels) =>
            {

            };
        }

        private object CurrentModelHost;
        private ModelNode CurrentModelNode;
        private int CurrentModelNodeIndex;

        private void ProcessModelDeployment(object modelHost, ModelNode modelNode)
        {
            CurrentModelNodeIndex = 1;

            CurrentModelHost = modelHost;
            CurrentModelNode = modelNode;

            using (new TraceActivityScope(TraceService, (int)LogEventId.ModelProcessing, string.Format("ProcessModelDeployment for model:[{0}]", modelNode.Value.GetType())))
            {
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
