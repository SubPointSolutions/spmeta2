using SPMeta2.Services;


namespace SPMeta2
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ServiceFactory
    {
        #region properties

        public abstract ModelServiceBase ModelService { get; set; }

        #endregion
    }
}
