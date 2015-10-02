﻿using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.ObjectModel;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint security role.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRoleDefinition", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RoleDefinition", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

    public class SecurityRoleDefinition : DefinitionBase
    {
        #region constructors

        public SecurityRoleDefinition()
        {
            BasePermissions = new Collection<string>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target security role.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]

        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        /// <summary>
        /// Description of the target security role.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// Permissions of the target security role.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsBasePermission]
        [DataMember]
        public Collection<string> BasePermissions { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Description:[{1}]", Name, Description);
        }

        #endregion
    }
}
