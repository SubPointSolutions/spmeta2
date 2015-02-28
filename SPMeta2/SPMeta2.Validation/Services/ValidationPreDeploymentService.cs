using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Validators.Collections;
using SPMeta2.Validation.Validators.Definitions;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Validation.Services
{
    public class ValidationPreDeploymentService : PreDeploymentServiceBase
    {
          #region constructors

        public ValidationPreDeploymentService()
        {
            InitValidators();
            Result = new List<ValidationResult>();
        }

        #endregion

        #region properties

        private List<ValidatorBase<DefinitionBase>> DefinitionValidators = new List<ValidatorBase<DefinitionBase>>();
        private List<RelationshipValidatorBase> RelationshipValidators = new List<RelationshipValidatorBase>();
        private List<CollectionValidatorBase> CollectionValidators = new List<CollectionValidatorBase>();

        public List<ValidationResult> Result { get; set; }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            Result.Clear();

            ProcessModelDeployment(modelHost, model, Result);
        }

        private void InitValidators()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            foreach (var vType in ReflectionUtils.GetTypesFromAssembly<DefinitionBaseValidator>(currentAssembly))
                DefinitionValidators.Add(Activator.CreateInstance(vType) as DefinitionBaseValidator);

            foreach (var vType in ReflectionUtils.GetTypesFromAssembly<RelationshipValidatorBase>(currentAssembly))
                RelationshipValidators.Add(Activator.CreateInstance(vType) as RelationshipValidatorBase);

            foreach (var vType in ReflectionUtils.GetTypesFromAssembly<CollectionValidatorBase>(currentAssembly))
                CollectionValidators.Add(Activator.CreateInstance(vType) as CollectionValidatorBase);
        }

        private void ProcessModelDeployment(object modelHost, ModelNode model, List<ValidationResult> result)
        {
            var modelNode = model;
            var modelDefinition = modelNode.Value;

            ValidateModelNode(modelNode, result);

            var childModelTypes = modelNode.ChildModels
                                       .Select(m => m.Value.GetType())
                                       .GroupBy(t => t);

            foreach (var childModelType in childModelTypes)
            {
                var childModels = modelNode.GetChildModels(childModelType.Key);

                ValidateModelNodeCollections(childModels, result);

                foreach (var childModel in childModels)
                    ProcessModelDeployment(modelHost, childModel, result);
            }
        }

        private void ValidateModelNode(ModelNode modelNode, List<ValidationResult> result)
        {
            var modelDefinition = modelNode.Value;

            foreach (var v in DefinitionValidators)
                v.Validate(modelNode.Value, result);

            foreach (var v in RelationshipValidators)
                v.Validate(modelNode, result);
        }

        private void ValidateModelNodeCollections(IEnumerable<ModelNode> modelNodeCollection, List<ValidationResult> result)
        {
            foreach (var v in CollectionValidators)
                v.Validate(modelNodeCollection.Select(m => m.Value), result);
        }

        #endregion
    }
}
