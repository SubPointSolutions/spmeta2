using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    public enum FeatureDefinitionScope
    {
        Farm,
        WebApplication,
        Site,
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
    [ExpectWithExtensionMethod]
    public class FeatureDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target feature.
        /// Is not used for any provision routines, can be omitted.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ID of the target features.
        /// </summary>
        /// 

        [ExpectValidation]
        public Guid Id { get; set; }

        /// <summary>
        /// ForceActivate flag which is passed to SPFeatureCollection.Add(is, forceActivate) method.
        /// </summary>
        /// 

        [ExpectValidation]
        public bool ForceActivate { get; set; }

        /// <summary>
        /// Enable or disable flag.
        /// Set 'true' to enable feature, set 'false' to disable feature.
        /// </summary>
        /// 

        [ExpectValidation]
        public bool Enable { get; set; }

        /// <summary>
        /// Scope of the target feature.
        /// </summary>
        /// 

        [ExpectValidation]
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
