using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Base
{
    public abstract class ReusableTextItemModelHandlerBase : ListItemModelHandler
    {
        protected override void MapListItemProperties(SPListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ReusableItemDefinitionBase>("model", value => value.RequireNotNull());

            newItem["Comments"] = definition.Comments ?? string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentCategory))
                newItem["ContentCategory"] = definition.ContentCategory;

            newItem["AutomaticUpdate"] = definition.AutomaticUpdate;
            newItem["ShowInRibbon"] = definition.ShowInDropDownMenu;
        }
    }
}
