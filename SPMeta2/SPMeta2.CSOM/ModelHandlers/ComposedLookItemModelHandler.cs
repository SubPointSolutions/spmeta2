using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ComposedLookItemModelHandler : ListItemModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemDefinition); }
        }

        protected override void MapListItemProperties(ListItem newItem, ListItemDefinition listItemModel)
        {
            base.MapListItemProperties(newItem, listItemModel);

            var definition = listItemModel.WithAssertAndCast<ComposedLookItemDefinition>("model", value => value.RequireNotNull());

            newItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInContentTypeId.ComposedLook;
            newItem["Name"] = definition.Name;

            SetUrlFieldValue(newItem, "MasterPageUrl", definition.MasterPageUrl, definition.MasterPageDescription);
            SetUrlFieldValue(newItem, "ThemeUrl", definition.ThemeUrl, definition.ThemeDescription);
            SetUrlFieldValue(newItem, "ImageUrl", definition.ImageUrl, definition.ImageDescription);
            SetUrlFieldValue(newItem, "FontSchemeUrl", definition.FontSchemeUrl, definition.FontSchemeDescription);

            if (definition.DisplayOrder.HasValue)
                newItem["DisplayOrder"] = definition.DisplayOrder.Value;

        }

        private void SetUrlFieldValue(ListItem newItem, string fieldName, string url, string description)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var urlFieldValue = new FieldUrlValue { Url = url };

                if (!string.IsNullOrEmpty(description))
                    urlFieldValue.Description = description;

                newItem[fieldName] = urlFieldValue;
            }
        }

        protected override ListItem FindListItem(List list, Folder folder, ListItemDefinition definition)
        {
            var context = list.Context;

            // by name
            var items = list.GetItems(new CamlQuery
            {
                FolderServerRelativeUrl = folder.ServerRelativeUrl,
                ViewXml = string.Format(@"<View>
                                          <Query>
                                             <Where>
                                                 <Eq>
                                                     <FieldRef Name='Name'/>
                                                     <Value Type='Text'>{0}</Value>
                                                 </Eq>
                                                </Where>
                                            </Query>
                                         </View>", definition.Title)
            });

            context.Load(items);
            context.ExecuteQueryWithTrace();

            if (items.Count > 0)
                return items[0];

            // by title
            items = list.GetItems(new CamlQuery
            {
                FolderServerRelativeUrl = folder.ServerRelativeUrl,
                ViewXml = string.Format(@"<View>
                                          <Query>
                                             <Where>
                                                 <Eq>
                                                     <FieldRef Name='Title'/>
                                                     <Value Type='Text'>{0}</Value>
                                                 </Eq>
                                                </Where>
                                            </Query>
                                         </View>", definition.Title)
            });

            context.Load(items);
            context.ExecuteQueryWithTrace();

            if (items.Count > 0)
                return items[0];

            return null;
        }
    }
}
