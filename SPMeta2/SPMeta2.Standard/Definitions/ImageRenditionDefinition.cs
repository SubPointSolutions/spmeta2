using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ImageRenditionDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectRequired]
        [DataMember]
        public int Height { get; set; }

        [ExpectRequired]
        [DataMember]
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
