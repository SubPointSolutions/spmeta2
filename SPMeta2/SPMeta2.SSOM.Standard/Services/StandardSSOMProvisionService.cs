using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;

namespace SPMeta2.SSOM.Standard.Services
{
    public class StandardSSOMProvisionService : SSOMProvisionService
    {
        #region constructors

        public StandardSSOMProvisionService()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
            RegisterModelHandlers(typeof(TaxonomyFieldModelHandler).Assembly);
        }

        #endregion
    }
}
