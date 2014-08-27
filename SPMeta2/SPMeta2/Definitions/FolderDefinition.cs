using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint folder.
    /// Can be deployed to web, list, folder and content type.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [RootHostAttribute(typeof(WebDefinition))]
    [ParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    public class FolderDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target folder.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
