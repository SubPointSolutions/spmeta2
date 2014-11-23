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
    public abstract class ModelServiceBase
    {
        #region constructors

        public ModelServiceBase()
        {
            TraceService = ServiceContainer.Instance.GetService<TraceServiceBase>();
            ModelWeighService = ServiceContainer.Instance.GetService<ModelWeighServiceBase>();

            using (new TraceActivityScope(TraceService, (int)LogEventId.Initialization,
                string.Format("Initializing new ModelServiceBase instance of type: [{0}]", GetType())))
            {
                ModelHandlers = new Dictionary<Type, ModelHandlerBase>();
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

            traceService.InformationFormat((int)LogEventId.CoreCalls,
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

        public TraceServiceBase TraceService { get; set; }
        public ModelWeighServiceBase ModelWeighService { get; set; }

        private readonly List<ModelHandlerBase> ModelHandlerEvents = new List<ModelHandlerBase>();
        private ModelNode _activeModelNode = null;

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
            TraceService.InformationFormat((int)LogEventId.CoreCalls,
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

        protected virtual IEnumerable<ModelWeigh> GetModelWeighs()
        {
            return ModelWeighService.GetModelWeighs();
        }

        #endregion

        #region public API

        public virtual void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            EnsureModelHandleEvents();
            ProcessModelDeployment(modelHost, model);
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

        private void ProcessModelDeployment(object modelHost, ModelNode modelNode)
        {
            using (new TraceActivityScope(TraceService, (int)LogEventId.ModelProcessing,
                        string.Format("ProcessModelDeployment for model:[{0}]", modelNode.Value.GetType())))
            {
                try
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "ProcessModelDeployment for model: [{0}]", new object[] { modelNode.Value });
                    TraceService.Verbose((int)LogEventId.ModelProcessing, "Calling OnModelNodeProcessingEventArgs event");

                    InvokeOnModelNodeProcessing(this, new OnModelNodeProcessingEventArgs
                    {
                        ModelNode = modelNode,
                        ModelHost = modelHost
                    });

                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Resolving model handler for model of type: [{0}]", new object[] { modelNode.Value.GetType() });

                    var modelDefinition = modelNode.Value as DefinitionBase;
                    var modelHandler = ResolveModelHandler(modelDefinition.GetType());

                    TraceService.Verbose((int)LogEventId.ModelProcessing, string.Format("Model handler instance is: {0}", modelHandler == null ? "NULL" : modelHandler.GetType().ToString()));

                    if (modelHandler == null)
                    {
                        TraceService.Error((int)LogEventId.ModelProcessingNullModelHandler, string.Format("Model handler instance is NULL. Throwing ArgumentNullException exception."));

                        throw new ArgumentNullException(
                            string.Format("Can't find model handler for type:[{0}]. Current ModelService type: [{1}].",
                                modelDefinition.GetType(),
                                GetType()));
                    }

                    TraceService.Information((int)LogEventId.ModelProcessing, string.Format("Model handler instance is: {0}", modelHandler.GetType()));

                    if (modelDefinition.RequireSelfProcessing || modelNode.Options.RequireSelfProcessing)
                    {
                        TraceService.Information((int)LogEventId.ModelProcessing, string.Format("RequireSelfProcessing = true. Starting deploying model."));
                        TraceService.Verbose((int)LogEventId.ModelProcessing, string.Format("RequireSelfProcessing = true. Starting deploying model."));

                        _activeModelNode = modelNode;

                        modelNode.State = ModelNodeState.Processing;
                        modelHandler.DeployModel(modelHost, modelDefinition);
                        modelNode.State = ModelNodeState.Processed;

                        _activeModelNode = null;

                        TraceService.Verbose((int)LogEventId.ModelProcessing, string.Format("RequireSelfProcessing = true. Finishing deploying model."));
                    }
                    else
                    {
                        TraceService.Information((int)LogEventId.ModelProcessing, string.Format("RequireSelfProcessing = false. Skipping deploying model."));
                    }

                    TraceService.Verbose((int)LogEventId.ModelProcessing, "Calling OnModelNodeProcessedEventArgs event.");

                    InvokeOnModelNodeProcessed(this, new OnModelNodeProcessedEventArgs
                    {
                        ModelNode = modelNode,
                        ModelHost = modelHost
                    });

                    TraceService.Information((int)LogEventId.ModelProcessing, "Sorting out child models by type and weigh.");

                    // sort out child models by types
                    var modelWeights = GetModelWeighs();

                    var childModelTypes = modelNode.ChildModels
                        .Select(m => m.Value.GetType())
                        .GroupBy(t => t)
                        .ToList();

                    var currentModelWeights = modelWeights.FirstOrDefault(w => w.Model == modelDefinition.GetType());

                    if (currentModelWeights != null)
                    {
                        childModelTypes.Sort(delegate(IGrouping<Type, Type> p1, IGrouping<Type, Type> p2)
                        {
                            var srcW = int.MaxValue;
                            var dstW = int.MaxValue;

                            // resolve model wight by current class or subclasses
                            // subclasses lookup is required for all XXX_FieldDefinitions 

                            // this to be extracted later as service 
                            var p1Type = currentModelWeights.ChildModels
                                .Keys.FirstOrDefault(k => k == p1.Key || k.IsAssignableFrom(p1.Key));

                            var p2Type = currentModelWeights.ChildModels
                                .Keys.FirstOrDefault(k => k == p2.Key || k.IsAssignableFrom(p2.Key));

                            if (p1Type != null)
                                srcW = currentModelWeights.ChildModels[p1Type];

                            if (p2Type != null)
                                dstW = currentModelWeights.ChildModels[p2Type];

                            return srcW.CompareTo(dstW);
                        });
                    }

                    TraceService.Information((int)LogEventId.ModelProcessing, "Processing child models.");

                    foreach (var childModelType in childModelTypes)
                    {
                        TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Starting processing child models of type: [{0}].", new object[] { childModelType.Key });

                        // V1, optimized one
                        // does not work with nintex workflow as 'List was modified and needs to be refreshed.'

                        //var childModels =
                        //        modelNode.GetChildModels(childModelType.Key);

                        //modelHandler.WithResolvingModelHost(modelHost, modelDefinition, childModelType.Key, childModelHost =>
                        //{
                        //    foreach (var childModel in childModels)
                        //        ProcessModelDeployment(childModelHost, childModel);
                        //});

                        // V2, less optimized version
                        var childModels = modelNode.GetChildModels(childModelType.Key);

                        TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Found [{0}] models of type: [{1}].", new object[] { childModels.Count(), childModelType.Key });

                        foreach (var childModel in childModels)
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Starting resolving model host of type: [{0}].", new object[] { childModelType.Key });

                            modelHandler.WithResolvingModelHost(new ModelHostResolveContext
                            {
                                ModelHost = modelHost,
                                Model = modelDefinition,
                                ChildModelType = childModelType.Key,
                                ModelNode = modelNode,
                                Action = childModelHost =>
                                {
                                    ProcessModelDeployment(childModelHost, childModel);
                                }
                            });

                            TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Finishing resolving model host of type: [{0}].", new object[] { childModelType.Key });
                        }

                        TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Finishing processing child models of type: [{0}].", new object[] { childModelType.Key });
                    }
                }
                catch (Exception modelProvisionException)
                {
                    TraceService.Error((int)LogEventId.ModelProcessingError, "Error while ProcessModelDeployment. Re-throw SPMeta2ModelDeploymentException.", modelProvisionException);

                    throw new SPMeta2ModelDeploymentException("Model provision error.", modelProvisionException)
                    {
                        ModelNode = modelNode
                    };
                }
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
