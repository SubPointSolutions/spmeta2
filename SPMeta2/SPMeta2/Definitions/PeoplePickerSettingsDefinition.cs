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
    /// Allows to define and deploy SharePoint People Picker Settings for SPWebApplication.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPPeoplePickerSettings", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(WebApplicationDefinition))]
    public class PeoplePickerSettingsDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Gets or sets a customized query filter to send to Active Directory Domain Services (ADDS).
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string ActiveDirectoryCustomFilter { get; set; }

        /// <summary>
        /// Gets or sets the custom query that is sent to Active Directory Domain Services.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string ActiveDirectoryCustomQuery { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether access to Active Directory is restricted when performing lookup of the user name.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? ActiveDirectoryRestrictIsolatedNameLevel { get; set; }

        /// <summary>
        /// Gets or sets the amount of time before Active Directory search times out.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public TimeSpan? ActiveDirectorySearchTimeout { get; set; }

        /// <summary>
        /// Gets or sets whether the People Picker control should resolve local accounts.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? AllowLocalAccount { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether the people picker should search Windows accounts when the current port uses forms authentication.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? NoWindowsAccountsForNonWindowsAuthenticationMode { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether to search only the current site collection.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? OnlySearchWithinSiteCollection { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the People Picker control should only perform lookup operations and resolve users from the site collection.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool? PeopleEditorOnlyResolveWithinSiteCollection { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ActiveDirectoryCustomFilter", ActiveDirectoryCustomFilter)
                          .AddRawPropertyValue("ActiveDirectoryCustomQuery", ActiveDirectoryCustomQuery)

                          .AddRawPropertyValue("ActiveDirectoryRestrictIsolatedNameLevel", ActiveDirectoryRestrictIsolatedNameLevel)
                          .AddRawPropertyValue("ActiveDirectorySearchTimeout", ActiveDirectorySearchTimeout)
                          .AddRawPropertyValue("AllowLocalAccount", AllowLocalAccount)

                          .AddRawPropertyValue("NoWindowsAccountsForNonWindowsAuthenticationMode", NoWindowsAccountsForNonWindowsAuthenticationMode)
                          .AddRawPropertyValue("OnlySearchWithinSiteCollection", OnlySearchWithinSiteCollection)
                          .AddRawPropertyValue("PeopleEditorOnlyResolveWithinSiteCollection", PeopleEditorOnlyResolveWithinSiteCollection)
                          .ToString();
        }

        #endregion
    }
}
