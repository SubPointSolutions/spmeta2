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
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy document set.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]


    [DefaultParentHost(typeof(ListDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]


    [ExpectManyInstances]

    public class DocumentSetDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public string Name { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectNullable]
        [ExpectUpdate]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "ContentType")]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "ContentType")]
        public string ContentTypeName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("ContentTypeId", ContentTypeId)
                          .AddRawPropertyValue("ContentTypeName", ContentTypeName)
                          .ToString();
        }

        #endregion
    }
}
