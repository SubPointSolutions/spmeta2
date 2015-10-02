﻿using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Definitions
{
    [DataContract]
    [Serializable]
    public enum FeatureDefinitionScope
    {
        [EnumMember]
        Farm,
        [EnumMember]
        WebApplication,
        [EnumMember]
        Site,
        [EnumMember]
        Web
    }

    /// <summary>
    /// Allows to define and deploy Farm, WebApplication, Site and Web features.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFeature", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Feature", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]


    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]
    public class FeatureDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target feature.
        /// Is not used for any provision routines, can be omitted.
        /// </summary>
        /// 
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// ID of the target features.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public Guid Id { get; set; }

        /// <summary>
        /// ForceActivate flag which is passed to SPFeatureCollection.Add(is, forceActivate) method.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        public bool ForceActivate { get; set; }

        /// <summary>
        /// Enable or disable flag.
        /// Set 'true' to enable feature, set 'false' to disable feature.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        public bool Enable { get; set; }

        /// <summary>
        /// Scope of the target feature.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        public FeatureDefinitionScope Scope { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<FeatureDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.Scope)
                          .AddPropertyValue(p => p.Enable)
                          .AddPropertyValue(p => p.ForceActivate)

                          .ToString();
        }

        #endregion
    }
}
