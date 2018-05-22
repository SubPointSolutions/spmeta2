using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Base
{
    [Serializable]
    [DataContract]
    public class NavigationNodePropertyValue
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// Base definition for SharePoint navigation nodes.
    /// </summary>
    /// 
    [Serializable]
    [DataContract]
    public abstract class NavigationNodeDefinitionBase : DefinitionBase
    {
        #region constructors

        protected NavigationNodeDefinitionBase()
        {
            IsVisible = true;
            TitleResource = new List<ValueForUICulture>();
            Properties = new List<NavigationNodePropertyValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target navigation node.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Title { get; set; }

        /// <summary>
        /// Corresponds to NameResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> TitleResource { get; set; }

        /// <summary>
        /// URL of the target navigation node.
        /// </summary>
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string Url { get; set; }

        /// <summary>
        /// IsExternal flag of the target navigation node.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool IsExternal { get; set; }

        /// <summary>
        /// IsVisible flag of the target navigation node.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool IsVisible { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<NavigationNodePropertyValue> Properties { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("Url", Url)
                          .AddRawPropertyValue("IsExternal", IsExternal)
                          .AddRawPropertyValue("IsVisible", IsVisible)
                          .ToString();
        }

        #endregion
    }
}
