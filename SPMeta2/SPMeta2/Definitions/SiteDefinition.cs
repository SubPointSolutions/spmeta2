using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint site collection.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Site", "Microsoft.SharePoint.Client")]

    [RootHostAttribute(typeof(WebApplicationDefinition))]
    [ParentHostAttribute(typeof(WebApplicationDefinition))]

    [Serializable]
    public class SiteDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target site collection.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the target site collection.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the target site collection.
        /// 
        /// URL should be without managed path.
        /// Use PrefixName property to setup managed path to create site collection under.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Name of the managed path to create site collection under.
        /// </summary>
        public string PrefixName { get; set; }

        /// <summary>
        /// Site template of the target site collection.
        /// 
        /// BuiltInWebTemplates class can be used to utilize out of the box SharePoint site templates.
        /// </summary>
        public string SiteTemplate { get; set; }

        /// <summary>
        /// LCID of the target site collection.
        /// </summary>
        public uint LCID { get; set; }

        /// <summary>
        /// Owner login name.
        /// </summary>
        public string OwnerLogin { get; set; }

        /// <summary>
        /// Owner name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Owner email.
        /// </summary>
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Secondary contact login.
        /// </summary>
        public string SecondaryContactLogin { get; set; }

        /// <summary>
        /// Secondary contact name.
        /// </summary>
        public string SecondaryContactName { get; set; }

        /// <summary>
        /// Secondary contact email.
        /// </summary>
        public string SecondaryContactEmail { get; set; }

        /// <summary>
        /// Database name of the target site collection.
        /// 
        /// If empty, site collection will be created in the default content database.
        /// If not empty, a new content database will be created.
        /// </summary>
        public string DatabaseName { get; set; }



        #endregion
    }
}
