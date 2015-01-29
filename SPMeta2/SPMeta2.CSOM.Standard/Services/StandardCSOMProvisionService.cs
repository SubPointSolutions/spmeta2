using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;

namespace SPMeta2.CSOM.Standard.Services
{
    public class StandardCSOMProvisionService : CSOMProvisionService
    {
        #region constructors

        public StandardCSOMProvisionService()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
            RegisterModelHandlers(typeof(TaxonomyFieldModelHandler).Assembly);
        }

        #endregion
    }
}
