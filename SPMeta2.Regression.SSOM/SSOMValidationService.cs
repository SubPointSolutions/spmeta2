using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.Regression.Validation.ServerModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;

namespace SPMeta2.Regression.SSOM
{
    public class SSOMValidationService : ModelServiceBase
    {
        public SSOMValidationService()
        {
            RegisterModelHandlers<SSOMModelHandlerBase>(this, GetType().Assembly);
        }
    }
}
