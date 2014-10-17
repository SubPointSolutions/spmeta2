using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Utils;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;


namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListDefinitionValidator : ListModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var context = web.Context;

            context.Load(web, w => w.ServerRelativeUrl);

            var lists = context.LoadQuery<List>(web.Lists.Include(l => l.DefaultViewUrl));
            context.ExecuteQuery();

            var spObject = FindListByUrl(lists, definition.GetListUrl());

            context.Load(spObject);
            context.Load(spObject, list => list.RootFolder.ServerRelativeUrl);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.Description, o => o.Description)
                .ShouldBeEndOf(m => m.GetServerRelativeUrl(web), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl())
                .ShouldBeEqual(m => m.ContentTypesEnabled, o => o.ContentTypesEnabled);

            if (definition.TemplateType > 0)
            {
                assert.ShouldBeEqual(m => m.TemplateType, o => (int)o.BaseTemplate);
            }
            else
            {
                assert.SkipProperty(m => m.TemplateType, "TemplateType == 0. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.TemplateName))
            {
                throw new SPMeta2NotImplementedException("TemplateName validation for List is not supported yet.");
            }
            else
            {
                assert.SkipProperty(m => m.TemplateName, "TemplateName is null or empty. Skipping.");
            }
        }
    }

    public static class Ex
    {
        public static string GetServerRelativeUrl(this ListDefinition listDef, Web web)
        {
            return UrlUtility.CombineUrl(web.ServerRelativeUrl, listDef.GetListUrl());
        }

        public static string GetServerRelativeUrl(this List list)
        {
            return list.RootFolder.ServerRelativeUrl;
        }

    }
}
