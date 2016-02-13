using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web part page.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class WebPartPageDefinition : PageDefinitionBase
    {
        #region constructors

        public WebPartPageDefinition()
        {
            NeedOverride = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Out of the box web part page layout id.
        /// 
        /// BuiltInWebpartPageTemplateId class can be used to utilize out of the box SharePoint web part page templates.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsWebPartPageLayoutTemplate]
        [ExpectRequired(GroupName = "Layout Template")]
        [DataMember]
        public int PageLayoutTemplate { get; set; }

        /// <summary>
        /// Custom web part page layout content.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Layout Template")]
        [DataMember]

        public string CustomPageLayout { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("{0} PageLayoutTemplate:[{1}]", base.ToString(), PageLayoutTemplate);
        }

        #endregion
    }
}
