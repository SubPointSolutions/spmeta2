using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint site collection.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Site", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebApplicationDefinition))]
    [DefaultParentHostAttribute(typeof(WebApplicationDefinition))]

    [CSOMRootHostAttribute(typeof(SiteDefinition))]
    [CSOMParentHostAttribute(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    public class SiteDefinition : DefinitionBase
    {
        #region constructors

        public SiteDefinition()
        {
            LCID = 1033;
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target site collection.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Description of the target site collection.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// URL of the target site collection.
        /// 
        /// URL should be without managed path.
        /// Use PrefixName property to setup managed path to create site collection under.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        /// <summary>
        /// Name of the managed path to create site collection under.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string PrefixName { get; set; }

        /// <summary>
        /// Site template of the target site collection.
        /// 
        /// BuiltInWebTemplates class can be used to utilize out of the box SharePoint site templates.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string SiteTemplate { get; set; }

        /// <summary>
        /// LCID of the target site collection.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public uint LCID { get; set; }

        /// <summary>
        /// Owner login name.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string OwnerLogin { get; set; }

        /// <summary>
        /// Owner name.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string OwnerName { get; set; }

        /// <summary>
        /// Owner email.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Secondary contact login.
        /// </summary>
        /// 
        [DataMember]
        public string SecondaryContactLogin { get; set; }

        /// <summary>
        /// Secondary contact name.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string SecondaryContactName { get; set; }

        /// <summary>
        /// Secondary contact email.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string SecondaryContactEmail { get; set; }

        /// <summary>
        /// Database name of the target site collection.
        /// 
        /// If empty, site collection will be created in the default content database.
        /// If not empty, a new content database will be created.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string DatabaseName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SiteDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.SiteTemplate)
                          .AddPropertyValue(p => p.PrefixName)
                          .AddPropertyValue(p => p.LCID)
                          .ToString();
        }

        #endregion
    }
}
