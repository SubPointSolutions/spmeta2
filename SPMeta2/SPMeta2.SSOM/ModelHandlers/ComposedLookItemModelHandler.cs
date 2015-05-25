using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ComposedLookItemModelHandler : ListItemModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemDefinition); }
        }

        protected override void MapListItemProperties(SPListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ComposedLookItemDefinition>("model", value => value.RequireNotNull());

            // composed look
            // 0x00EBB0D5D32C733345B0AA3C79625DD501
            newItem[SPBuiltInFieldId.ContentTypeId] = "0x00EBB0D5D32C733345B0AA3C79625DD501";
            newItem["Name"] = definition.Name;

            SetUrlFieldValue(newItem, "MasterPageUrl", definition.MasterPageUrl, definition.MasterPageDescription);
            SetUrlFieldValue(newItem, "ThemeUrl", definition.ThemeUrl, definition.ThemeDescription);
            SetUrlFieldValue(newItem, "ImageUrl", definition.ImageUrl, definition.ImageDescription);
            SetUrlFieldValue(newItem, "FontSchemeUrl", definition.FontSchemeUrl, definition.FontSchemeDescription);

            if (definition.DisplayOrder.HasValue)
                newItem["DisplayOrder"] = definition.DisplayOrder.Value;
        }

        private void SetUrlFieldValue(SPListItem newItem, string fieldName, string url, string description)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var urlFieldValue = new SPFieldUrlValue { Url = url };

                if (!string.IsNullOrEmpty(description))
                    urlFieldValue.Description = description;

                newItem[fieldName] = urlFieldValue;
            }
        }

        protected override SPListItem FindListItem(SPList list, SPFolder folder, ListItemDefinition listItemModel)
        {
            var definition = listItemModel.WithAssertAndCast<ComposedLookItemDefinition>("model", value => value.RequireNotNull());

            // first by Name
            var items = list.GetItems(new SPQuery
            {
                Folder = folder,
                Query = string.Format(@"<Where>
                             <Eq>
                                 <FieldRef Name='Name'/>
                                 <Value Type='Text'>{0}</Value>
                             </Eq>
                            </Where>", definition.Name)
            });

            if (items.Count > 0)
                return items[0];

            // by Title?
            items = list.GetItems(new SPQuery
            {
                Folder = folder,
                Query = string.Format(@"<Where>
                             <Eq>
                                 <FieldRef Name='Title'/>
                                 <Value Type='Text'>{0}</Value>
                             </Eq>
                            </Where>", definition.Title)
            });

            if (items.Count > 0)
                return items[0];

            return null;
        }
    }
}
