using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'List View' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]
    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.ListViewWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class ListViewWebPartDefinition : WebPartDefinition
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public string WebUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? WebId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public string ListTitle { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public string ListUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public Guid? ListId { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ViewName { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? ViewId { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ViewUrl { get; set; }

        [ExpectValidation]
        [ExpectUpdatAsToolbarType]
        [ExpectNullable]
        [DataMember]
        public string Toolbar { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [ExpectNullable]
        [DataMember]
        public bool? ToolbarShowAlways { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("ListTitle", ListTitle)
                          .AddRawPropertyValue("ListUrl", ListUrl)
                          .AddRawPropertyValue("ListId", ListId)

                          .AddRawPropertyValue("ViewName", ViewName)
                          .AddRawPropertyValue("ViewId", ViewId)
                          .ToString();
        }

        #endregion
    }
}
