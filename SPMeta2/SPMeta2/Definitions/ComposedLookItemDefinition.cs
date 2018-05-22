using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]
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
        [ExpectUpdateAsUrl(Extension = ".master")]
        [ExpectNullable]
        public string MasterPageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public string MasterPageDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsUrl(Extension = ".theme")]
        [ExpectNullable]
        public string ThemeUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public string ThemeDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsUrl(Extension = ".png")]
        [ExpectNullable]
        public string ImageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public string ImageDescription { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsUrl(Extension = ".fontsheme")]
        public string FontSchemeUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public string FontSchemeDescription { get; set; }


        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public int? DisplayOrder { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("Name", Name)
                          .ToString();
        }

        #endregion
    }
}
