using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.Standard.ModelHandlers.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class ReusableTextItemModelHandler : ReusableTextItemModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ReusableTextItemDefinition); }
        }

        protected override void MapListItemProperties(SPListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ReusableTextItemDefinition>("model", value => value.RequireNotNull());

            newItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInPublishingContentTypeId.ReusableText;
            newItem["ReusableText"] = definition.ReusableText;
        }
    }
}
