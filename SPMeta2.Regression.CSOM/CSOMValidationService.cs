using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOM
{
    public class CSOMValidationService : ModelServiceBase
    {
        #region constructors

        public CSOMValidationService()
        {
            RegisterModelHandlers<DefinitionValidatorBase>(this, typeof(CSOMValidationService).Assembly);
        }

        #endregion
    }
}
