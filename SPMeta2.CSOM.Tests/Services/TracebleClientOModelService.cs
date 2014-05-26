using SPMeta2.CSOM.Services;

namespace SPMeta2.CSOM.Tests.Services
{
    public class TracebleClientOModelService : CSOMProvisionService
    {
        #region contructors

        public TracebleClientOModelService()
        {
            // OnDeployingModel += (s, a) => Trace.WriteLine(string.Format("Deploying model: [{0}]", a.Model.ToString()));
            // OnDeployedModel += (s, a) => Trace.WriteLine(string.Format("Deployed model: [{0}]", a.Model.ToString()));
        }

        #endregion
    }
}
