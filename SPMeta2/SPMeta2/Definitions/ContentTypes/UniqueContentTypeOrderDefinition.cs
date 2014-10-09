using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    public class ContentTypeValue
    {
        public string ContentTypeName { get; set; }
        public string ContentTypeId { get; set; }
    }

    public class UniqueContentTypeOrderValue : ContentTypeValue
    {
    }

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    public class UniqueContentTypeOrderDefinition : DefinitionBase
    {
        #region constructors

        public UniqueContentTypeOrderDefinition()
        {
            ContentTypes = new List<UniqueContentTypeOrderValue>();
        }

        #endregion

        #region properties

        public List<UniqueContentTypeOrderValue> ContentTypes { get; set; }

        #endregion
    }
}
