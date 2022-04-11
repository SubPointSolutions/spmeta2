using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint folder.
    /// Can be deployed to web, list, folder and content type.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]
    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]

    [ExpectManyInstances]
    public class FolderDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target folder.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]

        [HashCodePartCapability]

        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)

                          .ToString();
        }

        #endregion
    }
}
