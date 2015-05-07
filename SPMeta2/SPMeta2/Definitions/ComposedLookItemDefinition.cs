using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    public class ComposedLookItemDefinition : ListItemDefinition
    {
        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public string MasterPageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string MasterPageDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ThemeUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ThemePageDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ImageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ImageDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        public string FontSchemeUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string FontSchemeDescription { get; set; }

        public int? DisplayOrder { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ComposedLookItemDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Name)
                          .ToString();
        }

        #endregion
    }
}
