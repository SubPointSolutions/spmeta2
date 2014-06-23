using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListDefinitionValidator : ListModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            TraceUtils.WithScope(traceScope =>
            {
                var spList = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, listModel.GetListUrl()));

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
                    trace.WriteLine(string.Format("Validate Url: model:[{0}] list:[{1}]", listModel.GetListUrl(), spList.RootFolder.ServerRelativeUrl));
                    Assert.IsTrue(spList.RootFolder.ServerRelativeUrl.Contains(listModel.GetListUrl()));
                });
            });
        }
    }
}
