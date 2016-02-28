using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.CSOM.Standard.Services
{
    public class StandardCSOMProvisionService : CSOMProvisionService
    {
        #region constructors

        public StandardCSOMProvisionService()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
            RegisterModelHandlers(typeof(ImageFieldModelHandler).Assembly);

            InitDefaultPreDeploymentServices(typeof(PublishingPageDefinition).Assembly);
            InitDefaultPreDeploymentServices(typeof(PublishingPageModelHandler).Assembly);
        }

        #endregion
    }
}
