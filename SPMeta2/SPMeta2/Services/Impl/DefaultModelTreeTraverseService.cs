using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.Models;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelTreeTraverseService : ModelTreeTraverseServiceBase
    {
        #region constructors

        #endregion

        #region properties

        private ModelWeighServiceBase _modelWeighService;

        public ModelWeighServiceBase ModelWeighService
        {
            get
            {
                if (_modelWeighService == null)
                    _modelWeighService = ServiceContainer.Instance.GetService<ModelWeighServiceBase>();

                return _modelWeighService;
            }
            set { _modelWeighService = value; }
        }

        #endregion

        #region methods

        public override void Traverse(object modelHost, ModelNode modelNode)
        {
            try
            {
                var modelDefinition = modelNode.Value as DefinitionBase;
                var modelHandler = OnModelHandlerResolve(modelNode);

                TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnModelFullyProcessing for model: [{0}].", modelNode);

                if (OnModelFullyProcessing != null)
                    OnModelFullyProcessing(modelNode);

                TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnModelProcessing for model: [{0}].", modelNode);

                if (OnModelProcessing != null)
                    OnModelProcessing(modelNode);

                var requireselfProcessing = modelDefinition.RequireSelfProcessing || modelNode.Options.RequireSelfProcessing;

                TraceService.InformationFormat((int)LogEventId.ModelProcessing, "Deploying model [{0}] RSP: [{1}] : [{2}].",
                    new[] { modelNode.Value.GetType().Name, requireselfProcessing.ToString(), modelNode.Value.ToString() });

                if (requireselfProcessing)
                {
                    modelHandler.DeployModel(modelHost, modelNode.Value);
                }

                TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnModelProcessed for model: [{0}].", modelNode);

                if (OnModelProcessed != null)
                    OnModelProcessed(modelNode);

                var childModelTypes = GetSortedChildModelTypes(modelNode);

                foreach (var childModelType in childModelTypes)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Starting processing child models of type: [{0}].", new object[] { childModelType.Key });

                    var childModels = modelNode.GetChildModels(childModelType.Key).ToList();
                    ModelWeighService.SortChildModelNodes(modelNode, childModels);

                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Found [{0}] models of type: [{1}].", new object[] { childModels.Count(), childModelType.Key });

                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnChildModelsProcessing of type: [{0}].", new object[] { childModelType.Key });

                    if (OnChildModelsProcessing != null)
                        OnChildModelsProcessing(modelNode, childModelType.Key, childModels);

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
                                Traverse(childModelHost, childModel);
                            }
                        });

                        TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Finishing resolving model host of type: [{0}].", new object[] { childModelType.Key });
                    }

                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnChildModelsProcessed of type: [{0}].", new object[] { childModelType.Key });
                    if (OnChildModelsProcessed != null)
                        OnChildModelsProcessed(modelNode, childModelType.Key, childModels);

                    TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Finishing processing child models of type: [{0}].", new object[] { childModelType.Key });
                }

                TraceService.VerboseFormat((int)LogEventId.ModelProcessing, "Raising OnModelFullyProcessed for model: [{0}].", modelNode);

                if (OnModelFullyProcessed != null)
                    OnModelFullyProcessed(modelNode);
            }
            catch (Exception e)
            {
                if (e is SPMeta2ModelDeploymentException)
                    throw;

                throw new SPMeta2ModelDeploymentException("There was an error while provisioning definition. Check ModelNode prop.", e)
                {
                    ModelNode = modelNode,
                };
            }
        }

        #endregion

        #region utils

        private IEnumerable<IGrouping<Type, Type>> GetSortedChildModelTypes(ModelNode modelNode)
        {
            var modelDefinition = modelNode.Value;
            var modelWeights = ModelWeighService.GetModelWeighs();

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
                    // stongly by type, and then by casting
                    var p1Type = currentModelWeights.ChildModels
                                                    .Keys.FirstOrDefault(k => k == p1.Key);

                    if (p1Type == null)
                    {
                        p1Type = currentModelWeights.ChildModels
                                                    .Keys.FirstOrDefault(k => k.IsAssignableFrom(p1.Key));
                    }

                    var p2Type = currentModelWeights.ChildModels
                                                    .Keys.FirstOrDefault(k => k == p2.Key);

                    if (p2Type == null)
                    {
                        p2Type = currentModelWeights.ChildModels
                                                    .Keys.FirstOrDefault(k => k.IsAssignableFrom(p2.Key));
                    }

                    if (p1Type != null)
                        srcW = currentModelWeights.ChildModels[p1Type];

                    if (p2Type != null)
                        dstW = currentModelWeights.ChildModels[p2Type];

                    return srcW.CompareTo(dstW);
                });
            }

            foreach (var g in childModelTypes)
            {

            }

            return childModelTypes;
        }

        #endregion
    }
}
