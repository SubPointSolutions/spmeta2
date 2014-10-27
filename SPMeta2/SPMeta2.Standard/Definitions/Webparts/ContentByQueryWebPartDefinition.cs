using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Content by Query' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    public class ContentByQueryWebPartDefinition : WebPartDefinition
    {
        #region properties

        public string DataMappings { get; set; }
        public string DataMappingViewFields { get; set; }

        public bool? PlayMediaInBrowser { get; set; }
        public bool? UseCopyUtil { get; set; }

        public int? ItemLimit { get; set; }

        public int? ServerTemplate { get; set; }

        public string WebUrl { get; set; }
        public Guid? ListGuid { get; set; }

        public string ItemStyle { get; set; }
        public string GroupStyle { get; set; }

        public bool? ShowUntargetedItems { get; set; }

        public string MainXslLink { get; set; }
        public string ItemXslLink { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ContentByQueryWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.DataMappings)
                          .AddPropertyValue(p => p.DataMappingViewFields)

                          .AddPropertyValue(p => p.ItemLimit)

                          .AddPropertyValue(p => p.WebUrl)
                          .AddPropertyValue(p => p.ListGuid)

                          .AddPropertyValue(p => p.ItemStyle)
                          .AddPropertyValue(p => p.GroupStyle)

                          .AddPropertyValue(p => p.MainXslLink)
                          .AddPropertyValue(p => p.ItemXslLink)

                          .ToString();
        }

        #endregion
    }
}
