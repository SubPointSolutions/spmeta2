using SPMeta2.Services;
using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Standard.Services.Impl.Validation
{
    public class DefaultPublishingPageDefinitionValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<BooleanFieldDefinitionValidationModelHandler>
    {
        #region constructors

        public DefaultPublishingPageDefinitionValidationService()
        {
            this.Title = "Publishing page validator";
            this.Description = "Ensures carrect values on publishing page definition";
        }

        #endregion
    }
}
