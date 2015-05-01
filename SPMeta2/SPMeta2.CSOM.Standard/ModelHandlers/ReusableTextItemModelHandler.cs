using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class ReusableTextItemModelHandler : ReusableTextItemModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ReusableTextItemDefinition); }
        }

        protected override void MapListItemProperties(ListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ReusableTextItemDefinition>("model", value => value.RequireNotNull());

            newItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInPublishingContentTypeId.ReusableText;
            newItem["ReusableText"] = definition.ReusableText;
        }
    }
}
