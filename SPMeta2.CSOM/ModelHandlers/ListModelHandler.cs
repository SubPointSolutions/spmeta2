using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListModelHandler : ModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var listDefinition = model as ListDefinition;

            if (web != null && listDefinition != null)
            {
                // TODO
                // no no no no... not a TITLE! 

                var context = web.Context;
                var list = web.QueryAndGetListByTitle(listDefinition.Title);

                if (childModelType == typeof(ListViewDefinition))
                {
                    context.Load<List>(list, l => l.Views);
                    context.ExecuteQuery();

                    action(list);
                }
                else if (childModelType == typeof(ModuleFileDefinition))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQuery();

                    var folderModelHost = new FolderModelHost
                    {
                        CurrentWeb = web,
                        CurrentList = list,
                        CurrentLibraryFolder = list.RootFolder
                    };

                    action(folderModelHost);
                }
                else if (childModelType == typeof(FolderDefinition))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQuery();

                    var folderModelHost = new FolderModelHost();

                    folderModelHost.CurrentWeb = web;
                    folderModelHost.CurrentList = list;

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentLibraryFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListItem = null;
                    }

                    action(folderModelHost);
                }
                else if (childModelType == typeof(SP2013WorkflowSubscriptionDefinition))
                {
                    var sp2013WorkflowSubscriptionModelHost = ModelHostBase.Inherit<SP2013WorkflowSubscriptionModelHost>(webModelHost, host =>
                    {
                        host.HostList = list;
                    });

                    action(sp2013WorkflowSubscriptionModelHost);
                }
                else
                {
                    action(list);
                }

                list.Update();
            }
            else
            {
                action(modelHost);
            }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            context.Load(web, w => w.Lists);
            context.Load(web, w => w.ServerRelativeUrl);

            context.ExecuteQuery();

            List currentList = null;

            InvokeOnModelEvents<ListDefinition, List>(currentList, ModelEventType.OnUpdating);

            // gosh!
            currentList = FindListByTitle(web.Lists, listModel.Title);

            if (currentList == null)
            {
                // no support for the TemplateName yet
                var listInfo = new ListCreationInformation
                {
                    Title = listModel.Title,
                    Description = listModel.Description ?? string.Empty,
                    Url = listModel.GetListUrl()
                };

                if (listModel.TemplateType > 0)
                {
                    listInfo.TemplateType = listModel.TemplateType;
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    context.Load(web, tmpWeb => tmpWeb.ListTemplates);
                    context.ExecuteQuery();

                    // gosh..
                    var listTemplate = FindListTemplateByName(web.ListTemplates, listModel.TemplateName);

                    listInfo.TemplateFeatureId = listTemplate.FeatureId;
                    listInfo.TemplateType = listTemplate.ListTemplateTypeKind;
                }
                else
                {
                    throw new ArgumentException("Either TemplateType or TemplateName has to bbe specified.");
                }

                currentList = web.Lists.Add(listInfo);
            }

            currentList.Title = listModel.Title;
            currentList.Description = listModel.Description ?? string.Empty;
            currentList.ContentTypesEnabled = listModel.ContentTypesEnabled;

            InvokeOnModelEvents<ListDefinition, List>(currentList, ModelEventType.OnUpdated);


            currentList.Update();

            context.ExecuteQuery();
        }

        protected List FindListByTitle(ListCollection listCollection, string listTitle)
        {
            foreach (var list in listCollection)
            {
                if (System.String.Compare(list.Title, listTitle, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return list;
            }

            return null;
        }

        protected ListTemplate FindListTemplateByName(IEnumerable<ListTemplate> listTemplateCollection, string listTemplateName)
        {
            foreach (var listTemplate in listTemplateCollection)
            {
                if (System.String.Compare(listTemplate.Name, listTemplateName, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return listTemplate;
            }

            return null;
        }

        #endregion

        public override Type TargetType
        {
            get { return typeof(ListDefinition); }
        }
    }
}
