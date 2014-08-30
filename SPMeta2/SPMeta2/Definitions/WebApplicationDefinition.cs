using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web application.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPWebApplication", "Microsoft.SharePoint")]

    [RootHostAttribute(typeof(FarmDefinition))]
    [ParentHostAttribute(typeof(FarmDefinition))]

    [Serializable]

    public class WebApplicationDefinition : DefinitionBase
    {
        /// <summary>
        /// Application pool is of the target web application.
        /// </summary>
        public string ApplicationPoolId { get; set; }

        /// <summary>
        /// Application pool user name.
        /// </summary>
        public string ApplicationPoolUsername { get; set; }

        /// <summary>
        /// Application pool password.
        /// </summary>
        public string ApplicationPoolPassword { get; set; }

        /// <summary>
        /// Port number of the target web application.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Host header of the target web application.
        /// </summary>
        public string HostHeader { get; set; }

        /// <summary>
        /// Create new database flag.
        /// </summary>
        public bool CreateNewDatabase { get; set; }

        /// <summary>
        /// AllowAnonymousAccess flag.
        /// </summary>
        public bool AllowAnonymousAccess { get; set; }

        /// <summary>
        /// Managed account to run application pool of the target web application.
        /// </summary>
        public string ManagedAccount { get; set; }

        /// <summary>
        /// UseSecureSocketsLayer flag.
        /// </summary>
        public bool UseSecureSocketsLayer { get; set; }

        /// <summary>
        /// Database name of the target web application.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Database server of the target web application.
        /// </summary>
        public string DatabaseServer { get; set; }

        /// <summary>
        /// Should NTLM authentication be used.
        /// </summary>
        public bool UseNTLMExclusively { get; set; }
    }
}
