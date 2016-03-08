using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultBooleanFieldDefinitionValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<BooleanFieldDefinitionValidationModelHandler>
    {
        #region constructors

        public DefaultBooleanFieldDefinitionValidationService()
        {
            this.Title = "Boolean field defaul value validator";
            this.Description = "Ensures correct default value for boolean field type";
        }

        #endregion
    }
}
