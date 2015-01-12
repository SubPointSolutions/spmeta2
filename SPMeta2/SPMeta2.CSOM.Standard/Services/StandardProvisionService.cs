using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;

namespace SPMeta2.CSOM.Standard.Services
{
    public class StandardSSOMProvisionService : CSOMProvisionService
    {
        #region constructors

        public StandardSSOMProvisionService()
        {
            RegisterModelHandlers(typeof(TaxonomyFieldModelHandler).Assembly);
        }

        #endregion
    }
}
