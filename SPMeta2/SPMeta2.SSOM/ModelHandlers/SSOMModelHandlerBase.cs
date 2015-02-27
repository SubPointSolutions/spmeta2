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
        }

        #endregion

        #region properties

        public TokenReplacementServiceBase TokenReplacementService { get; set; }

        #endregion
    }
}
