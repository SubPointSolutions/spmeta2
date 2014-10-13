
using SPMeta2.Containers.Services;

namespace SPMeta2.Regression.Tests.Services
{
    internal static class ServiceFactory
    {
        #region static

        static ServiceFactory()
        {
            ModelGeneratorService = new ModelGeneratorService();
        }

        #endregion

        #region properties

        public static ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion
    }
}
