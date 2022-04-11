namespace SPMeta2.Services
{
    public abstract class SPMetaServiceBase
    {
        #region constructors

        #endregion

        #region properties

        private TraceServiceBase _traceService;

        public virtual TraceServiceBase TraceService
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
