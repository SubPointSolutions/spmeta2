using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.SSOM.Standard.Services
{
    public class StandardSSOMProvisionService : SSOMProvisionService
    {
        #region constructors

        public StandardSSOMProvisionService()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
            RegisterModelHandlers(typeof(TaxonomyFieldModelHandler).Assembly);

            InitDefaultPreDeploymentServices(typeof(PublishingPageDefinition).Assembly);
            InitDefaultPreDeploymentServices(typeof(PublishingPageModelHandler).Assembly);
        }

        #endregion
    }
}
