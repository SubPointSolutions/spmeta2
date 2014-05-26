using System;

namespace SPMeta2.Regression.Profiles
{
    public class WebApplicationSandboxProfile : TestProfileBase
    {
        #region static

        public WebApplicationSandboxProfile()
        {
            ProfileName = "Default";
            WebApplicationUrl = string.Format("http://{0}", Environment.MachineName);
            ManagedPath = "auto-tests";
        }

        #endregion

        #region properties

        public string ProfileName { get; set; }

        public string WebApplicationUrl { get; set; }
        public string ManagedPath { get; set; }

        #endregion
    }
}
