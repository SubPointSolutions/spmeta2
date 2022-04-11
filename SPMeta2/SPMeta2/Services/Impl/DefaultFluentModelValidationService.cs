using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services.Impl
{
    public class DefaultFluentModelValidationService : FluentModelValidationServiceBase
    {
        #region methods

        public override ModelValidationResultSet<DefinitionBase> Validate(ModelNode node, Action<TypedModelValidationResult<DefinitionBase>> action)
        {
            return Validate<DefinitionBase>(node, action);
        }


        public override ModelValidationResultSet<TDefinition> Validate<TDefinition>(ModelNode node, Action<TypedModelValidationResult<TDefinition>> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var result = new ModelValidationResultSet<TDefinition>();

            WalkModel(result, null, node, action, validationContext =>
            {
                return validationContext.ValidationContext.CurrentDefinition is TDefinition;
            });

            return result;
        }

        private void WalkModel<TDefinition>(ModelValidationResultSet<TDefinition> result, ModelNode parentModelNode, ModelNode node,
            Action<TypedModelValidationResult<TDefinition>> action,
            Func<TypedModelValidationResult<TDefinition>, bool> filterCallback)
              where TDefinition : DefinitionBase
        {
            var validationResult = new TypedModelValidationResult<TDefinition>
            {

            };

            validationResult.ValidationContext.ParentModelNode = parentModelNode;
            validationResult.ValidationContext.CurrentModelNode = node;

            validationResult.ValidationContext.ParentDefinition = parentModelNode != null ? parentModelNode.Value : null;
            validationResult.ValidationContext.CurrentDefinition = node.Value as TDefinition;

            validationResult.ValidationContext.ChildModelNodes = node.ChildModels;
            validationResult.ValidationContext.ChildDefinitions = node.ChildModels.Where(m => m.Value is TDefinition).Select(m => m.Value as TDefinition);

            if (filterCallback != null)
            {
                if (filterCallback(validationResult))
                {
                    action(validationResult);
                    result.ValidationResults.Add(validationResult);
                }
            }
            else
            {
                action(validationResult);
                result.ValidationResults.Add(validationResult);
            }

            foreach (var childNode in node.ChildModels)
                WalkModel(result, node, childNode, action, filterCallback);
        }

        #endregion
    }
}
