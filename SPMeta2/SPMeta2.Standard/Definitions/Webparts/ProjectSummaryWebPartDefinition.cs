using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'ProjectSummaryWebPart' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    public class ProjectSummaryWebPartDefinition : WebPartDefinition
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public string PrimaryTaskListUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? ListId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ProjectSummaryWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.PrimaryTaskListUrl)
                          .AddPropertyValue(p => p.ListId)
                          .ToString();
        }

        #endregion
    }
}
