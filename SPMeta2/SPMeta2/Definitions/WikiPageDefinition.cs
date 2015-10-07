using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

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
    [DataContract]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class WikiPageDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target page.
        /// Is not used by the SharePoint for wiki pages.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        // Title is not used by the wiki pages and SharePoint
        // Should not be required
        // https://github.com/SubPointSolutions/spmeta2/issues/684
        //[ExpectRequired]
        [DataMember]
        public override string Title { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
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
