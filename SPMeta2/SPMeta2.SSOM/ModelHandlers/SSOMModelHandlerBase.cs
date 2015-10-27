using System.ComponentModel;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Services;
using SPMeta2.SSOM.Services;


namespace SPMeta2.SSOM.ModelHandlers
{
    public abstract class SSOMModelHandlerBase : ModelHandlerBase
    {
        #region constructors

        public SSOMModelHandlerBase()
        {
            TokenReplacementService = ServiceContainer.Instance.GetService<SSOMTokenReplacementService>();
            LocalizationService = ServiceContainer.Instance.GetService<SSOMLocalizationService>();

            // TODO, move to the ServiceContainer
            ContentTypeLookupService = new SSOMContentTypeLookupService();
            FieldLookupService = new SSOMFieldLookupService();
        }

        #endregion

        #region properties

        protected SSOMFieldLookupService FieldLookupService { get; set; }
        protected SSOMContentTypeLookupService ContentTypeLookupService { get; set; }

        public TokenReplacementServiceBase TokenReplacementService { get; set; }
        public LocalizationServiceBase LocalizationService { get; set; }

        #endregion
    }
}
