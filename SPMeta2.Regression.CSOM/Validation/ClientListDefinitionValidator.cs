using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListDefinitionValidator : ListModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var context = web.Context;

            context.Load(web, w => w.ServerRelativeUrl);
            context.Load(web, w => w.Lists);
            context.ExecuteQuery();

            var spObject = FindListByTitle(web.Lists, listModel.Title);

            context.Load(spObject, list => list.RootFolder.ServerRelativeUrl);
            context.ExecuteQuery();

            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<ListDefinition, List>(listModel, spObject);

                traceScope.WriteLine(string.Format("Validating model:[{0}] field:[{1}]", model, spObject));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, o => o.Title)
                    .ShouldBeEqual(trace, m => m.Description, o => o.Description)
                    .ShouldBeEqual(trace, m => m.GetServerRelativeUrl(web), o => o.GetServerRelativeUrl())
                    .ShouldBeEqual(trace, m => m.ContentTypesEnabled, o => o.ContentTypesEnabled));

                if (listModel.TemplateType > 0)
                {
                    traceScope.WithTraceIndent(trace => pair
                        .ShouldBeEqual(trace, m => m.TemplateType, o => (int)o.BaseTemplate));
                }
                else
                {

                }
            });
        }
    }

    public static class Ex
    {
        public static string GetServerRelativeUrl(this ListDefinition listDef, Web web)
        {
            return web.ServerRelativeUrl + "/" + listDef.GetListUrl();
        }

        public static string GetServerRelativeUrl(this List list)
        {
            return list.RootFolder.ServerRelativeUrl;
        }

    }
}
