using System;
using System.Collections.Generic;
using SPMeta2.Definitions;

namespace SPMeta2.Common
{
    [Obsolete]
    public class ModelHandlerContext
    {
        #region properties

        //public ServiceFactory ServiceFactory { get; set; }

        public object ModelHost { get; set; }
        public IEnumerable<DefinitionBase> Models { get; set; }

        #endregion
    }
}
