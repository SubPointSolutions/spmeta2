using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Base
{
    public abstract class ReusableTextItemModelHandlerBase : ListItemModelHandler
    {
        protected override void MapListItemProperties(ListItem newItem, ListItemDefinition listItemModel)
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
