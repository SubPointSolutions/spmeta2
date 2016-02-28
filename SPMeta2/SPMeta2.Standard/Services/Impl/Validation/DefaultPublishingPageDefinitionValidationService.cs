using SPMeta2.Services;
using SPMeta2.Standard.Services.ServiceModelHandlers;

namespace SPMeta2.Standard.Services.Impl.Validation
{
    public class DefaultPublishingPageDefinitionValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<PublishingPageDefinitionValidationModelHandler>
    {
        #region constructors

        public DefaultPublishingPageDefinitionValidationService()
        {
            this.Title = "Publishing page validator";
            this.Description = "Ensures correct values on publishing page definition";
        }

        #endregion
    }
}
