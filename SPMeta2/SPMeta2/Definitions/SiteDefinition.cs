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
    /// Allows to define and deploy SharePoint site collection.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Site", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [CSOMRootHost(typeof(SiteDefinition))]
    [CSOMParentHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [ExpectArrayExtensionMethod]
    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(WebApplicationDefinition))]

    [ExpectManyInstances]

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
        [ExpectRequired]
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
        [ExpectRequired]
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
        [ExpectRequired]
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
        [ExpectRequired]
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
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Url", Url)
                          .AddRawPropertyValue("SiteTemplate", SiteTemplate)
                          .AddRawPropertyValue("PrefixName", PrefixName)
                          .AddRawPropertyValue("LCID", LCID)
                          .ToString();
        }

        #endregion
    }
}
