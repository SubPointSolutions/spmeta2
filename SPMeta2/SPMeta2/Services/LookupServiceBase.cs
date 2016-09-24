using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    public class LookupServiceBase
    {
        #region constructors

        #endregion

        #region properties

        public virtual TraceServiceBase TraceService
        {
            get
            {
                return ServiceContainer.Instance.GetService<TraceServiceBase>();
            }
        }

        #endregion

        #region methods


        #endregion
    }
}
