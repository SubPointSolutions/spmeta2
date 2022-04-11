using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOM
{
    public class CSOMValidationService : ModelServiceBase
    {
        #region constructors

        public CSOMValidationService()
        {
            RegisterModelHandlers<CSOMModelHandlerBase>(this, GetType().Assembly);
        }

        #endregion
    }
}
