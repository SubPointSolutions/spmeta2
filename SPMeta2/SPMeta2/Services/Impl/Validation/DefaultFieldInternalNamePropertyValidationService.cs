using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultFieldInternalNamePropertyValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultFieldInternalNamePropertyModelHandler>
    {
        #region constructors

        public DefaultFieldInternalNamePropertyValidationService()
        {
            this.Title = "Field Internal Name validator";
            this.Description = "Ensures correct field internal name value";
        }

        #endregion
    }
}
