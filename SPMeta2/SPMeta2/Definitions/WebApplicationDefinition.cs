using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web application.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPWebApplication", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(FarmDefinition))]
    public class WebApplicationDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Application pool is of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ApplicationPoolId { get; set; }

        /// <summary>
        /// Application pool user name.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ApplicationPoolUsername { get; set; }

        /// <summary>
        /// Application pool password.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ApplicationPoolPassword { get; set; }

        /// <summary>
        /// Port number of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public int Port { get; set; }

        /// <summary>
        /// Host header of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string HostHeader { get; set; }

        /// <summary>
        /// Create new database flag.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool CreateNewDatabase { get; set; }

        /// <summary>
        /// AllowAnonymousAccess flag.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool AllowAnonymousAccess { get; set; }

        /// <summary>
        /// Managed account to run application pool of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ManagedAccount { get; set; }

        /// <summary>
        /// UseSecureSocketsLayer flag.
        /// </summary>
        /// 
        [DataMember]
        public bool UseSecureSocketsLayer { get; set; }

        /// <summary>
        /// Database name of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Database server of the target web application.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string DatabaseServer { get; set; }

        /// <summary>
        /// Should NTLM authentication be used.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool UseNTLMExclusively { get; set; }

        #endregion


        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebApplicationDefinition>(this)
                          .AddPropertyValue(p => p.HostHeader)
                          .AddPropertyValue(p => p.Port)

                          .AddPropertyValue(p => p.CreateNewDatabase)
                          .AddPropertyValue(p => p.DatabaseServer)
                          .AddPropertyValue(p => p.DatabaseName)

                          .AddPropertyValue(p => p.ApplicationPoolId)
                          .AddPropertyValue(p => p.UseSecureSocketsLayer)
                          .AddPropertyValue(p => p.ManagedAccount)
                          .ToString();
        }

        #endregion
    }
}
