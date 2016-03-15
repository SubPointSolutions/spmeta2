using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultRequiredPropertiesValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultRequiredPropertiesModelHandler>
    {
        #region constructors

        public DefaultRequiredPropertiesValidationService()
        {
            this.Title = "Required values validator";
            this.Description = "Ensures required values are set";
        }

        #endregion
    }
}
