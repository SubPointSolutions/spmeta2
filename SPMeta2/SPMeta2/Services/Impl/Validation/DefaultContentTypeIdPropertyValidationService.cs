using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultContentTypeIdPropertyValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultContentTypeIdPropertyModelHandler>
    {
        #region constructors

        public DefaultContentTypeIdPropertyValidationService()
        {
            this.Title = "Content Type ID validator";
            this.Description = "Ensures correct content type ID value";
        }

        #endregion
    }
}
