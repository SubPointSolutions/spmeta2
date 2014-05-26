using SPMeta2.Services;

namespace SPMeta2
{
    public abstract class ServiceFactory
    {
        #region properties

        public abstract ModelServiceBase ModelService { get; set; }

        #endregion
    }
}
