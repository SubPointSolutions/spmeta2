using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Services;

namespace SPMeta2.Regression.MinimalModelTests.Base
{
    public enum ModelHostScope
    {
        SPSite,
        SPWeb
    }

    public enum ProvisionEngineSettingsScope
    {
        CSOM_ClientContext,
        O365_ClientContext,
        SSOM_SPSite,
    }

    public class ProvisionEngineSettings
    {
        public string Name { get; set; }

        public ModelServiceBase ValidationService { get; set; }
        public ModelServiceBase ProvisionService { get; set; }

        public bool IsEnabled { get; set; }
        public ProvisionEngineSettingsScope Scope { get; set; }
    }
}
