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
    /// Allows too define and deploy SharePoint web site.
    /// </summary>

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class WebDefinition : DefinitionBase
    {
        #region constructors

        public WebDefinition()
        {
            Url = "/";
            LCID = 1033;
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Description of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// LCID of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        //[ExpectUpdateAsLCID]
        public uint LCID { get; set; }

        /// <summary>
        /// Should unique permissions be used for the new web.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool UseUniquePermission { get; set; }

        /// <summary>
        /// Convert to the new web template if web if there.
        /// </summary>
        /// 

        [DataMember]
        public bool ConvertIfThere { get; set; }

        /// <summary>
        /// URL of the target. 
        /// Should be relative to the current web. 
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        /// <summary>
        /// WebTemplate of the target web.
        /// 
        /// BuiltInWebTemplates class can be used to utilize out of the box SharePoint web templates.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web Template")]
        [DataMember]
        public string WebTemplate { get; set; }

        /// <summary>
        /// Custom web template name of the target web.
        /// </summary>
        [ExpectRequired(GroupName = "Web Template")]
        [DataMember]
        public string CustomWebTemplate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.LCID)
                          .AddPropertyValue(p => p.UseUniquePermission)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.WebTemplate)
                          .AddPropertyValue(p => p.CustomWebTemplate)

                          .ToString();
        }

        #endregion
    }
}