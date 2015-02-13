using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    public abstract class SPMetaServiceBase
    {
        #region constructors

        public SPMetaServiceBase()
        {
        }

        #endregion

        #region properties

        private TraceServiceBase _traceService;

        public TraceServiceBase TraceService
        {
            get
            {
                if (_traceService == null)
                    _traceService = ServiceContainer.Instance.GetService<TraceServiceBase>();

                return _traceService;
            }
            set { _traceService = value; }
        }

        #endregion

        #region methods


        #endregion
    }
}
