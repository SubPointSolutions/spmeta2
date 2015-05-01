using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class ReusableHTMLItemModelHandler : ReusableTextItemModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ReusableHTMLItemDefinition); }
        }

        protected override void MapListItemProperties(ListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ReusableHTMLItemDefinition>("model", value => value.RequireNotNull());

            newItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInPublishingContentTypeId.ReusableHtml;
            newItem["ReusableHtml"] = definition.ReusableHTML;
        }
    }
}
