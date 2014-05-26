using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListDefinitionValidator : ListModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var web = modelHost.WithAssertAndCast<Web>("modelHost", value => value.RequireNotNull());
            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            TraceUtils.WithScope(traceScope =>
            {
                var context = web.Context;

                context.Load(web, w => w.Lists);
                context.ExecuteQuery();

                // gosh!
                var spList = FindListByTitle(web.Lists, listModel.Title);

                context.Load(spList, list => list.RootFolder.ServerRelativeUrl);
                context.ExecuteQuery();

                traceScope.WriteLine(string.Format("Validate model:[{0}] field:[{1}]", listModel, spList));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Title: model:[{0}] list:[{1}]", listModel.Title, spList.Title));
                    Assert.AreEqual(listModel.Title, spList.Title);

                    trace.WriteLine(string.Format("Validate Description: model:[{0}] list:[{1}]", listModel.Description, spList.Description));
                    Assert.AreEqual(listModel.Description, spList.Description);

                    trace.WriteLine(string.Format("Validate ContentTypesEnabled: model:[{0}] list:[{1}]", listModel.ContentTypesEnabled, spList.ContentTypesEnabled));
                    Assert.AreEqual(listModel.ContentTypesEnabled, spList.ContentTypesEnabled);

                    // TODO
                    // template type & template name
                    if (listModel.TemplateType > 0)
                    {
                        trace.WriteLine(string.Format("Validate TemplateType: model:[{0}] list:[{1}]", listModel.TemplateType, spList.BaseTemplate));
                        Assert.AreEqual(listModel.TemplateType, (int)spList.BaseTemplate);
                    }
                    else
                    {
                        trace.WriteLine(string.Format("Skipping TemplateType check. It is 0"));
                    }

                    // TODO
                    // url checking
                    trace.WriteLine(string.Format("Validate URL: model:[{0}] list:[{1}]", listModel.GetListUrl(), spList.RootFolder.ServerRelativeUrl));
                    Assert.IsTrue(spList.RootFolder.ServerRelativeUrl.Contains(listModel.GetListUrl()));
                });
            });
        }
    }
}
