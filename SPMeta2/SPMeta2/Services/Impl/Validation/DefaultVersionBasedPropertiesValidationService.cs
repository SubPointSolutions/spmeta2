using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultVersionBasedPropertiesValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultVersionBasedPropertiesModelHandler>
    {
        #region constructors

        public DefaultVersionBasedPropertiesValidationService()
        {
            this.Title = "Version value validator";
            this.Description = "Ensures correct version value";
        }

        #endregion
    }
}
