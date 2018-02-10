using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Services
{
    public abstract class WebPartPageTemplatesServiceBase
    {
        #region methods

        public abstract string GetPageLayoutTemplate(int pageLayoutTemplate, Version spRuntimeVersion);

        #endregion
    }
}
