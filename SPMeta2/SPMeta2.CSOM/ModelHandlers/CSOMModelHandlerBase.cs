using SPMeta2.CSOM.Services;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Services;


namespace SPMeta2.CSOM.ModelHandlers
{
    public abstract class CSOMModelHandlerBase : ModelHandlerBase
    {
        #region constructors

        public CSOMModelHandlerBase()
        {
            TokenReplacementService = ServiceContainer.Instance.GetService<CSOMTokenReplacementService>();
        }

        #endregion

        #region properties

        public TokenReplacementServiceBase TokenReplacementService { get; set; }

        #endregion
    }
}
