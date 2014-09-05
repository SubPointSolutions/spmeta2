using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web part.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebPartPageDefinition))]

    [Serializable]

    public class WebPartDefinition : DefinitionBase
    {
        #region contructors

        public WebPartDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string Title { get; set; }

        /// <summary>
        /// ID of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string Id { get; set; }

        /// <summary>
        /// ZoneId of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public string ZoneId { get; set; }

        /// <summary>
        /// ZoneIndex of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]

        public int ZoneIndex { get; set; }

        /// <summary>
        /// File name of the target web part definition from the web part gallery.
        /// 
        /// WebpartFileName is used for the first priority to deploy web part.
        /// </summary>
        /// 
        [ExpectValidation]
        public string WebpartFileName { get; set; }

        /// <summary>
        /// Type of the target web part.
        /// 
        /// WebpartType is used as a second priority to deploy web part.
        /// </summary>
        /// 
        [ExpectValidation]
        public string WebpartType { get; set; }

        /// <summary>
        /// XML definition of the target web part.
        /// Both V2 and V3 definition are supported.
        /// 
        /// WebpartXmlTemplate is used as the final step to deploy web part. 
        /// </summary>        
        /// 
        [ExpectValidation]
        public string WebpartXmlTemplate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] Id:[{1}] WebpartFileName:[{2}] WebpartType:[{3}] ZoneId:[{4}] ZoneIndex:[{5}]",
                new[] { Title, Id, WebpartFileName, WebpartType, ZoneId, ZoneIndex.ToString() });
        }

        #endregion
    }
}
