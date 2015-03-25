using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.Utils;


namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class WebPartGalleryFileModelHandler : ContentFileModelHandlerBase
    {
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

        protected override void MapProperties(object modelHost, SPListItem item, ContentPageDefinitionBase definition)
        {
            var typedDefinition = definition.WithAssertAndCast<WebPartGalleryFileDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedDefinition.Group))
                item["Group"] = typedDefinition.Group;

            if (!string.IsNullOrEmpty(typedDefinition.Description))
                item["WebPartDescription"] = typedDefinition.Description;


            var recSettingsValue = new SPFieldMultiChoiceValue();

            foreach (var value in typedDefinition.RecommendationSettings)
                recSettingsValue.Add(value);

            item["QuickAddGroups"] = recSettingsValue;
        }

        public override Type TargetType
        {
            get { return typeof(WebPartGalleryFileDefinition); }
        }
    }
}
