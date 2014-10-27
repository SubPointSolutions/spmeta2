using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint wiki page.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]

    public class WikiPageDefinition : PageDefinitionBase
    {
        #region properties

        public string Content { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WikiPageDefinition>(this, base.ToString())
                         
                          .ToString();
        }

        #endregion
    }
}
