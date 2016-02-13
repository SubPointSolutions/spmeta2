using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultNotAbsoluteUrlPropertiesValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultNotAbsoluteUrlPropertiesModelHandler>
    {
        #region constructors

        public DefaultNotAbsoluteUrlPropertiesValidationService()
        {
            this.Title = "Non absolute URL value validator";
            this.Description = "Ensures URL has non-absolute value";
        }

        #endregion
    }
}
