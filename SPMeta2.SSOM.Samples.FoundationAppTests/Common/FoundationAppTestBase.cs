using SPMeta2.Regression.Base;
using SPMeta2.SSOM.Samples.FoundationAppTests.Services;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Common
{
    public class FoundationAppTestBase : SPWebApplicationSandboxTest
    {
        #region properties

        protected virtual string TestSiteCollectionTitle
        {
            get { return "spmeta-foundation-app"; }
        }

        private readonly AppModelService _modelService = new AppModelService();

        protected virtual AppModelService ModelService
        {
            get { return _modelService; }
        }

        #endregion
    }
}
