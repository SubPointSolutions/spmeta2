using System;
using System.Collections.Generic;
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
        #region constructors

        public WebApplicationDefinition()
        {
            AllowedInlineDownloadedMimeTypes = new List<string>();
        }

        #endregion

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

        /// <summary>
        /// Maps AllowedInlineDownloadedMimeTypes value for SPWebApplication
        /// By default adds all values on top of existing once
        /// Use ShouldOverrideAllowedInlineDownloadedMimeTypes to force override this property array
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public List<string> AllowedInlineDownloadedMimeTypes { get; set; }

        /// <summary>
        /// Indicates if AllowedInlineDownloadedMimeTypes property should overwrite existing values in SPWebApplication
        /// Null, false - values will be added on top
        /// true - existing values are deleted and new values are added
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? ShouldOverrideAllowedInlineDownloadedMimeTypes { get; set; }

        #endregion


        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("HostHeader", HostHeader)
                          .AddRawPropertyValue("Port", Port)

                          .AddRawPropertyValue("CreateNewDatabase", CreateNewDatabase)
                          .AddRawPropertyValue("DatabaseServer", DatabaseServer)
                          .AddRawPropertyValue("DatabaseName", DatabaseName)

                          .AddRawPropertyValue("ApplicationPoolId", ApplicationPoolId)
                          .AddRawPropertyValue("UseSecureSocketsLayer", UseSecureSocketsLayer)
                          .AddRawPropertyValue("ManagedAccount", ManagedAccount)
                          .ToString();
        }

        #endregion
    }
}
