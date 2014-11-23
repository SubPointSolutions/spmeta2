using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ListViewWebPartModelHandler : WebPartModelHandler
    {
        #region classes

        internal class ListBindContext
        {
            public Guid ListId { get; set; }
            public string TitleUrl { get; set; }

            public Guid ViewId { get; set; }
        }

        #endregion

        #region properties

        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var bindContext = LookupBindContext(listItemModelHost, wpModel);

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.ListViewWebPart)
                                         .SetOrUpdateListVieweWebPartProperty("ListName", bindContext.ListId.ToString("B"))
                                         .SetOrUpdateListVieweWebPartProperty("ListId", bindContext.ListId.ToString("D").ToLower())
                                         .SetOrUpdateListVieweWebPartProperty("ViewGuid", bindContext.ViewId.ToString("D").ToLower())
                                         .SetTitleUrl(bindContext.TitleUrl)
                                         .ToString();

            return wpXml;
        }

        private ListBindContext LookupBindContext(ListItemModelHost listItemModelHost, ListViewWebPartDefinition wpModel)
        {
            var result = new ListBindContext
            {

            };

            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            List list = null;

            if (!string.IsNullOrEmpty(wpModel.ListUrl))
            {
                list = web.QueryAndGetListByUrl(wpModel.ListUrl);

            }
            else if (!string.IsNullOrEmpty(wpModel.ListTitle))
            {
                list = web.Lists.GetByTitle(wpModel.ListTitle);
            }
            else if (wpModel.ListId != default(Guid))
            {
                list = web.Lists.GetById(wpModel.ListId.Value);
            }
            else
            {
                throw new ArgumentException("ListUrl, ListTitle or ListId should be defined.");
            }

            context.Load(list, l => l.Id);
            context.Load(list, l => l.DefaultViewUrl);
            context.Load(list, l => l.Title);
            context.Load(list, l => l.DefaultView);

            context.ExecuteQueryWithTrace();

            result.ListId = list.Id;
            result.TitleUrl = list.DefaultViewUrl;
            result.ViewId = list.DefaultView.Id;

            return result;
        }

        #endregion
    }
}
