﻿using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy property value to the SharePoint property bags.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "System.Object", "mscorlib")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "System.Object", "mscorlib")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]


    [ParentHostCapability(typeof(FarmDefinition))]
    [ParentHostCapability(typeof(WebApplicationDefinition))]
    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]
    [ParentHostCapability(typeof(ListItemDefinition))]

    [ExpectManyInstances]

    public class PropertyDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target property.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Key { get; set; }

        /// <summary>
        /// Value of the target property.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// Should value be overwritten
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool Overwrite { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PropertyDefinition>(this)
                          .AddPropertyValue(p => p.Key)
                          .AddPropertyValue(p => p.Value)
                          .AddPropertyValue(p => p.Overwrite)

                          .ToString();
        }

        #endregion
    }
}
