using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Validators.Collections;
using SPMeta2.Validation.Validators.Definitions;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Validation.Services
{
    public class ModelValidationService : ModelServiceBase
    {
        public ModelValidationService()
        {
            InitValidators();
            Result = new List<ValidationResult>();
        }

        private List<ValidatorBase<DefinitionBase>> DefinitionValidators = new List<ValidatorBase<DefinitionBase>>();
        private List<RelationshipValidatorBase> RelationshipValidators = new List<RelationshipValidatorBase>();
        private List<CollectionValidatorBase> CollectionValidators = new List<CollectionValidatorBase>();

        private static IEnumerable<Type> GetTypes<TType>()
        {
            return (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from lType in lAssembly.GetTypes()
                    where typeof(TType).IsAssignableFrom(lType) && !lType.IsAbstract
                    select lType).ToArray();
        }

        public List<ValidationResult> Result { get; set; }

        public override void DeployModel(object modelHost, ModelNode model)
        {
            Result.Clear();

            ProcessModelDeployment(modelHost, model, Result);
        }

        private void InitValidators()
        {
            foreach (var v in GetTypes<DefinitionBaseValidator>())
                DefinitionValidators.Add(Activator.CreateInstance(v) as DefinitionBaseValidator);

            foreach (var v in GetTypes<RelationshipValidatorBase>())
                RelationshipValidators.Add(Activator.CreateInstance(v) as RelationshipValidatorBase);

            foreach (var v in GetTypes<CollectionValidatorBase>())
                CollectionValidators.Add(Activator.CreateInstance(v) as CollectionValidatorBase);

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
    }
}
