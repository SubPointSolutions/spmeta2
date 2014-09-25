using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Runners.Config
{
    public class EnvironmentConfig
    {
        #region constructors

        public EnvironmentConfig()
        {
            WebApplications = new List<WebApplicationConfig>();
        }

        #endregion

        #region prop

        public List<WebApplicationConfig> WebApplications { get; set; }

        #endregion
    }

    public class WebApplicationConfig
    {
        #region constructors

        public WebApplicationConfig()
        {
            SiteCollections = new List<SiteCollectionConfig>();
        }

        #endregion

        #region properties

        public string Url { get; set; }
        public int Port { get; set; }

        public List<SiteCollectionConfig> SiteCollections { get; set; }

        #endregion
    }

    public class SiteCollectionConfig
    {
        #region constructors

        public SiteCollectionConfig()
        {
            Webs = new List<WebConfig>();
        }

        #endregion

        #region properties

        public string Url { get; set; }
        public string Prefix { get; set; }

        public List<WebConfig> Webs { get; set; }

        #endregion
    }

    public class WebConfig
    {
        public string Url { get; set; }
    }

}
