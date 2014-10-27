using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint image rendition.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.ImageRendition", "Microsoft.SharePoint.Publishing")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Publishing.ImageRendition", "Microsoft.SharePoint.Client.Publishing")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class ImageRenditionDefinition : DefinitionBase
    {
        #region properties

        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ImageRenditionDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Height)
                          .AddPropertyValue(p => p.Width)
                          .ToString();
        }

        #endregion
    }
}
