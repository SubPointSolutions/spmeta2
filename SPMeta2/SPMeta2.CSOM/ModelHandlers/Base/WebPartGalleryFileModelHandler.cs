using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Base
{
    public class WebPartGalleryFileModelHandler : ContentFileModelHandlerBase
    {
        #region properties

        public override string FileExtension
        {
            get
            {
                return Path.GetExtension(CurrentModel.FileName)
                           .Replace(".", string.Empty);
            }
            set
            {

            }
        }

        public override Type TargetType
        {
            get { return typeof(WebPartGalleryFileDefinition); }
        }

        #endregion

        #region methods

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            var typedDefinition = definition.WithAssertAndCast<WebPartGalleryFileDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedDefinition.Group))
                item["Group"] = typedDefinition.Group;

            if (!string.IsNullOrEmpty(typedDefinition.Description))
                item["WebPartDescription"] = typedDefinition.Description;

            item["QuickAddGroups"] = typedDefinition.RecommendationSettings.ToArray();
        }

        #endregion
    }
}
