using SPMeta2.Services.ServiceModelHandlers;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultXmlBasedPropertiesValidationService :
        DefaultPreDeploymentTreeTraverseServiceBase<DefaultXmlBasedPropertiesModelHandler>
    {
        #region constructors

        public DefaultXmlBasedPropertiesValidationService()
        {
            this.Title = "XML value validator";
            this.Description = "Ensures correct XML value";
        }

        #endregion
    }
}
