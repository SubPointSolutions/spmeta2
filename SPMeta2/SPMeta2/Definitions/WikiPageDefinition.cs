using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint wiki page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

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
            return new ToStringResultRaw(base.ToString())

                          .ToString();
        }

        #endregion
    }
}
