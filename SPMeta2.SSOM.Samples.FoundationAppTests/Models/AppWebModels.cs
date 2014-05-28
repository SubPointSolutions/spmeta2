using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppWebModels
    {
        #region properties

        public static WebDefinition Projects = new WebDefinition
        {
            Title = "Projects",
            Description = "Some project site",
            WebTemplate = "STS#0",
            Url = "projects"
        };

        public static WebDefinition P1 = new WebDefinition
        {
            Title = "p1",
            Description = "Some project site",
            WebTemplate = "STS#0",
            Url = "p1"
        };

        public static WebDefinition P2 = new WebDefinition
        {
            Title = "p2",
            Description = "Some project site",
            WebTemplate = "STS#0",
            Url = "p2"
        };

        public static WebDefinition Teams = new WebDefinition
        {
            Title = "Teams",
            Description = "Some team site",
            WebTemplate = "STS#0",
            Url = "teams"
        };

        #endregion
    }
}
